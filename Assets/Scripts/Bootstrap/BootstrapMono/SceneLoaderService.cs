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
}

public class SceneLoaderService : MonoBehaviour, IService
{
	[SerializeField] private List<Level> _levels = new List<Level>();

	private Level _activeLevel;
	private string _newScene;
	private Level _nextLevel;


	public static event Action OnSceneLoadCompleted;

	public static event Action OnLevelEnded;

	public async Task InitializeAsync()
	{
		Debug.Log("SceneLoaderService Initialize");
		_newScene = "LevelOne";
		_nextLevel = _levels[0];
		
		await Task.CompletedTask;
	}

	// public void EndLevelEvent(string nextScene)
	// {
	// 	_newScene = nextScene;
	// 	OnLevelEnded?.Invoke();
	// }

	public void SetNextLevel()
	{
		int currentIndex = _levels.IndexOf(_activeLevel);

		if (currentIndex >= 0 && currentIndex < _levels.Count - 1)
		{
			_nextLevel = _levels[currentIndex + 1];
			OnLevelEnded?.Invoke();
		}
		else
		{
			Debug.Log("No more levels!");
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
			Debug.Log("There is no such level!");
		}
	}

	// public async void Test()
	// {
	// 	await LoadLevel(_newScene);
	// }

	// Главный метод для смены уровня
	public async Task LoadLevel(string levelName)
	{
		Level newLevel = _levels.Find(l => l.LevelName == levelName);
		if (newLevel == null)
		{
			Debug.LogError($"Level {levelName} not found!");
			return;
		}

		// Сначала выгружаем сцены, которых не будет в новом уровне
		await UnloadScenesNotInLevel(newLevel);

		// Затем загружаем новые сцены
		await LoadNewScenes(newLevel);

		_activeLevel = newLevel;
		OnSceneLoadCompleted?.Invoke();
	}
	
	public async Task LoadLevel()
	{		
		if (_activeLevel == _nextLevel) return;

		// Сначала выгружаем сцены, которых не будет в новом уровне
		await UnloadScenesNotInLevel(_nextLevel);

		// Затем загружаем новые сцены
		await LoadNewScenes(_nextLevel);

		_activeLevel = _nextLevel;
		OnSceneLoadCompleted?.Invoke();
	}

	// Выгружаем сцены, которых не будет в новом уровне
	private async Task UnloadScenesNotInLevel(Level newLevel)
	{
		if (_activeLevel == null) return;

		foreach (SceneData oldScene in _activeLevel.Scenes)
		{
			// Проверяем, есть ли эта сцена в новом уровне
			bool sceneExistsInNewLevel = newLevel.Scenes.Exists(s => s.Name == oldScene.Name);

			if (!sceneExistsInNewLevel)
			{
				Debug.Log($"Unloading scene: {oldScene.Name}");
				SceneHelper.UnloadScene(oldScene.Name);
			}
		}

		await Task.CompletedTask;
	}

	// Загружаем новые сцены
	private async Task LoadNewScenes(Level newLevel)
	{
		foreach (SceneData scene in newLevel.Scenes)
		{
			Debug.Log($"Checking scene: {scene.Name}");

			// Пропускаем, если это уже активная сцена
			if (SceneManager.GetActiveScene().name == scene.Name)
			{
				Debug.Log($"Scene {scene.Name} is already active, skipping");
				continue;
			}

			// Пропускаем, если она уже загружена
			if (SceneManager.GetSceneByName(scene.Name).isLoaded)
			{
				Debug.Log($"Scene {scene.Name} is already loaded, skipping");
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

			// await Task.Delay(TimeSpan.FromSeconds(2.5f));
		}

		await Task.CompletedTask;
	}

	// Вспомогательный метод для получения текущего активного уровня
	public Level GetActiveLevel() => _activeLevel;

	// Вспомогательный метод для получения всех уровней
	public List<Level> GetAllLevels() => _levels;
}

// public interface Test
// {
// 	public void LoadNewLevel();
// 	public void UnloadLevel();
// 	public Level GetActiveLevel();
// 	public bool AllSceneLoad(); // или это ивент

// }