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
	// public SceneData ActiveScene => _scenes.FirstOrDefault(s => s.IsActiveScene);
}

public interface ISceneLoaderService
{
	void LoadLevel(SceneReference levelName);
}

public class SceneLoaderService : MonoBehaviour, IService
{
	[SerializeField] private List<Level> _levels = new List<Level>();

	private Level _activeLevel;
	private string _newScene;


	public static event Action OnSceneLoadCompleted;

	public static event Action OnLevelEnded; //TODO В другом сркрипте

	public async Task InitializeAsync()
	{
		Debug.Log("SceneLoaderService Initialize");

		_newScene = "LevelOne";

		await Task.CompletedTask;
	}

	public void EndLevelEvent(string nextScene)
	{
		_newScene = nextScene;
		OnLevelEnded?.Invoke();
	}

	public void Test()
	{
		LoadLevel(_newScene);
		// LoadLevel("LevelOne");

	}

	public async void LoadLevel(string levelName)
	{
		Level level = _levels.Find(l => l.LevelName == levelName);
		_activeLevel = level;

		foreach (SceneData scene in level.Scenes)
		{
			Debug.Log($"Scene - {scene.Name} loaded!");

			// Пропускаем, если это уже активная сцена
			if (SceneManager.GetActiveScene().name == scene.Name)
				continue;

			// Пропускаем, если она уже есть в Additive
			if (SceneManager.GetSceneByName(scene.Name).isLoaded)
				continue;

			// await SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);

			if (scene.IsActiveScene)
				SceneHelper.LoadScene(scene.Name, additive: true, setActive: true);

			else
				SceneHelper.LoadScene(scene.Name, additive: true);


			// await Task.Delay(5000);

			await Task.CompletedTask;
		}

		OnSceneLoadCompleted.Invoke();
	}

	// public async void Unloadlevel(string levelName)
	// {
	// 	if (_activeLevel == null) return;

	// 	Level newlevel = _levels.Find(l => l.LevelName == levelName);

	// 	foreach (SceneData scene in _activeLevel.Scenes)
	// 	{
	// 		if (newlevel.Scenes.Contains(scene)) return;

	// 		SceneHelper.UnloadScene(scene.Name);
	// 	}

	// 	await Task.CompletedTask;
	// }
}
