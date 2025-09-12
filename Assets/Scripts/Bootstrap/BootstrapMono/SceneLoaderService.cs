using System.Collections.Generic;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level
{
	[SerializeField] private string _levelName;
	[SerializeField] private List<SceneReference> _scenes = new List<SceneReference>();

	public string LevelName => _levelName;
	public List<SceneReference> Scenes => _scenes;
}

public interface ISceneLoaderService
{
	void LoadLevel(SceneReference levelName);
}

public class SceneLoaderService : MonoBehaviour, IService
{
	[SerializeField] private List<Level> _levels = new List<Level>();

	public async Task InitializeAsync()
	{
		Debug.Log("SceneLoaderService Initialize");
		await Task.CompletedTask;

	}

	public void Test()
	{
		LoadLevel("LevelOne");
	}

	public async void LoadLevel(string levelName)
	{
		Level level = _levels.Find(l => l.LevelName == levelName);
		
		foreach (SceneReference scene in level.Scenes)
		{
			// Пропускаем, если это уже активная сцена
			if (SceneManager.GetActiveScene().name == scene.Name)
				continue;

			// Пропускаем, если она уже есть в Additive
			if (SceneManager.GetSceneByName(scene.Name).isLoaded)
				continue;

			// await SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
			SceneHelper.LoadScene(scene.Name, additive: true);


			await Task.CompletedTask;
		}
	}
}
