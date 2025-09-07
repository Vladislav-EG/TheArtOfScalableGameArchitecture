using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrappedData : MonoBehaviour
{
	public static BootstrappedData Instance { get; private set; } = null;

	void Awake()
	{
		// check if an instance already exists
		if (Instance != null)
		{
			Debug.LogError("Found another BootstrappedData on " + gameObject.name);
			Destroy(gameObject);
			return;
		}

		Debug.Log("Bootstrap initialised!");
		Instance = this;
		
		// TestWithDelay();

		// prevent the data from being unloaded
		DontDestroyOnLoad(gameObject);
	}

	public void Test()
	{
		Debug.Log("Bootstrap is working!");
	}
	
	// public Task TestWithDelay()
	// {
	// 	return Task.Delay(10000);
	// }
}

public static class PerformBootstrap
{
	const string SceneName = "BootstrapScene";

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Execute()
	{
		// traverse the currently loaded scenes
		for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; ++sceneIndex)
		{
			var candidate = SceneManager.GetSceneAt(sceneIndex);

			// early out if already loaded
			if (candidate.name == SceneName)
				return;
		}
				

		Debug.Log("Loading bootstrap scene: " + SceneName);

		// additively load the bootstrap scene
		SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
	}
}

// public static class PerformBootstrap
// {
//     const string SceneName = "BootstrapScene";

//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//     public static void Execute()
//     {
//         for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; ++sceneIndex)
//         {
//             var candidate = SceneManager.GetSceneAt(sceneIndex);
//             if (candidate.name == SceneName)
//                 return;
//         }

//         Debug.Log("Loading bootstrap scene: " + SceneName);
//         var op = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
//         op.completed += _ => Debug.Log("Bootstrap scene fully loaded!");
//     }
// }
