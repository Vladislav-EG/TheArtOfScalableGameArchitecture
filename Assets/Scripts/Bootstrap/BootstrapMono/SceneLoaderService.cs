using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneData
{
	[SerializeField] private SceneReference _scene;
	[SerializeField] private bool _isActiveScene = false;

	public SceneReference Scene => _scene;
	public bool IsActiveScene => _isActiveScene;
	public string Name => _scene.Name;
}

[System.Serializable]
public class Level
{
	[SerializeField] private string _levelName;
	[SerializeField] private List<SceneData> _scenes = new List<SceneData>();

	public string LevelName => _levelName;
	public List<SceneData> Scenes => _scenes;
}

public interface ISceneLoaderService
{
	Task LoadLevel(string levelName);
	Task LoadNextLevel();
	void SetNextLevel();
	void SetNextLevel(string levelName);
	Level GetActiveLevel();
	List<Level> GetAllLevels();

	static event Action OnLevelLoadCompleted;
	static event Action OnLevelEnded;
}

public class SceneLoaderService : MonoBehaviour, IService, ISceneLoaderService
{
	[SerializeField] private List<Level> _levels = new List<Level>();

	private Level _activeLevel;
	private Level _nextLevel;

	public static event Action OnLevelLoadCompleted;
	public static event Action OnLevelEnded;

	public async Task InitializeAsync()
	{
		Debug.Log("SceneLoaderService Initialize");

		if (_levels.Count > 0)
		{
			_nextLevel = _levels[0];
		}

		await Task.CompletedTask;
	}

	public void SetNextLevel()
	{
		if (_activeLevel == null)
		{
			Debug.LogWarning("No active level set!");
			return;
		}

		int currentIndex = _levels.IndexOf(_activeLevel);

		if (currentIndex >= 0 && currentIndex < _levels.Count - 1)
		{
			_nextLevel = _levels[currentIndex + 1];
			OnLevelEnded?.Invoke();
		}
		else
		{
			Debug.Log("No more levels available!");
		}
	}

	public void SetNextLevel(string levelName)
	{
		var nextLevel = _levels.Find(l => l.LevelName == levelName);

		if (nextLevel != null)
		{
			_nextLevel = nextLevel;
			OnLevelEnded?.Invoke();
		}
		else
		{
			Debug.LogError($"Level '{levelName}' not found!");
		}
	}

	public async Task LoadLevel(string levelName)
	{
		Level targetLevel = _levels.Find(l => l.LevelName == levelName);

		if (targetLevel == null)
		{
			Debug.LogError($"Level '{levelName}' not found!");
			return;
		}

		await LoadLevelInternal(targetLevel);
	}

	public async Task LoadNextLevel()
	{
		if (_nextLevel == null)
		{
			Debug.LogWarning("No next level set!");
			return;
		}

		if (_activeLevel == _nextLevel)
		{
			Debug.Log("Next level is the same as active level, skipping load.");
			return;
		}

		await LoadLevelInternal(_nextLevel);
	}

	private async Task LoadLevelInternal(Level targetLevel)
	{
		Debug.Log($"Loading level: {targetLevel.LevelName}");

		await UnloadScenesNotInLevel(targetLevel);
		await LoadNewScenes(targetLevel);

		_activeLevel = targetLevel;

		Debug.Log($"Level '{targetLevel.LevelName}' loaded successfully");
		
		OnLevelLoadCompleted?.Invoke();

	}

	private async Task UnloadScenesNotInLevel(Level newLevel)
	{
		if (_activeLevel == null) return;

		foreach (SceneData oldScene in _activeLevel.Scenes)
		{
			bool sceneExistsInNewLevel = newLevel.Scenes.Exists(s => s.Name == oldScene.Name);

			if (!sceneExistsInNewLevel && SceneManager.GetSceneByName(oldScene.Name).isLoaded)
			{
				Debug.Log($"Unloading scene: {oldScene.Name}");
				SceneHelper.UnloadScene(oldScene.Name);
			}
		}

		await Task.CompletedTask;
	}

	private async Task LoadNewScenes(Level newLevel)
	{
		foreach (SceneData scene in newLevel.Scenes)
		{
			if (IsSceneAlreadyLoadedOrActive(scene.Name))
			{
				Debug.Log($"Scene '{scene.Name}' is already loaded, skipping");
				continue;
			}

			Debug.Log($"Loading scene: {scene.Name}");

			if (scene.IsActiveScene)
			{
				SceneHelper.LoadScene(scene.Name, additive: true, setActive: true);
			}
			else
			{
				SceneHelper.LoadScene(scene.Name, additive: true);
			}

			await Task.Delay(500);

		}

		await Task.CompletedTask;
	}

	private bool IsSceneAlreadyLoadedOrActive(string sceneName)
	{
		return SceneManager.GetActiveScene().name == sceneName ||
			   SceneManager.GetSceneByName(sceneName).isLoaded;
	}

	public Level GetActiveLevel() => _activeLevel;
	public List<Level> GetAllLevels() => _levels;
}