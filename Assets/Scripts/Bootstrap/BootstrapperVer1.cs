// using UnityEditor;
// using UnityEditor.SceneManagement;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class Bootstrapper : MonoBehaviour
// {
// 	[SerializeField] private string defaultScene = "MainMenu"; // Сцена по умолчанию

// 	private void Awake()
// 	{
// 		DontDestroyOnLoad(gameObject);
// 		InitializeServices();

// 		// Проверяем, была ли сцена запущена напрямую
// 		if (SceneManager.sceneCount == 1)
// 		{
// 			LoadScene(defaultScene);
// 		}
// 	}

// 	private void InitializeServices()
// 	{
// 		// Здесь можно подключать DI контейнер (Zenject/Extenject или свой),
// 		// настраивать менеджеры, сервисы, загрузку конфигов и т.д.
// 		Debug.Log("Services initialized");
// 	}

// 	public void LoadScene(string sceneName)
// 	{
// 		SceneManager.LoadScene(sceneName);
// 	}
// }


// // [InitializeOnLoad]
// // public static class BootstrapLoader
// // {
// // 	static BootstrapLoader()
// // 	{
// // 		EditorApplication.playModeStateChanged += OnPlayModeChanged;
// // 	}

// // 	private static void OnPlayModeChanged(PlayModeStateChange state)
// // 	{
// // 		if (state == PlayModeStateChange.ExitingEditMode)
// // 		{
// // 			if (SceneManager.GetActiveScene().name != "Bootstrap")
// // 			{
// // 				EditorPrefs.SetString("PlayFromScene", SceneManager.GetActiveScene().path);
// // 				EditorSceneManager.OpenScene("Assets/Scenes/Bootstrap.unity");
// // 			}
// // 		}

// // 		if (state == PlayModeStateChange.EnteredPlayMode)
// // 		{
// // 			if (EditorPrefs.HasKey("PlayFromScene"))
// // 			{
// // 				string scenePath = EditorPrefs.GetString("PlayFromScene");
// // 				EditorPrefs.DeleteKey("PlayFromScene");

// // 				string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
// // 				var bootstrapper = Object.FindObjectOfType<Bootstrapper>();
// // 				if (bootstrapper != null)
// // 				{
// // 					bootstrapper.LoadScene(sceneName);
// // 				}
// // 			}
// // 		}
// // 	}
// // }


