using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO Поедлить на отдельные скрипты которые будут выполнять свою суть
// TODO Какой-нибудь момет с инвтом разобрать GameStateMachine.cs - Bootstrapper.OnBootstrapCompleted += OnBootstrapCompleted;



/* TODO 

1. Грузится бустраппер со всеми асинхронными сервисами и стейтмашиной
2. Дальше стейт машина переходит из состояния "Boostrap" в состояние "LoadLevelState"

Bootstrap to LoadLevelState
Gameplay to LoadLevelState

3. В состоянии "LoadLevelState" SceneLoaderService аддативно загружает все сцены нужного уровня
4. Запускается след состояние ("GameplayState")


У меня есть Level, в теории он может быть SO

   */

public class Bootstrapper : MonoBehaviour
{
	[SerializeField] private string SceneName = "SampleScene";

	[SerializeField] private List<MonoBehaviour> _monoServices = new List<MonoBehaviour>();

	[SerializeField] private List<ScriptableObject> _scriptableServices = new List<ScriptableObject>();

	public static event Action OnBootstrapCompleted;
	
	private SceneLoaderService _sceneLoaderService;

	private List<Type> _plainServiceTypes = new List<Type>
	{
		// typeof(TestService),
		// typeof(TestService2)

	};

	private List<IService> _allServices = new List<IService>();

	private async void Awake()
	{
		CollectServices();
		RegisterServices();
		await InitializeServices();

		Debug.Log("The loading of services is completed");

		// SceneManager.LoadScene(SceneName, LoadSceneMode.Additive); // TODO: SceneLoaderService
		
		// await Task.Delay(2000);
		

		OnBootstrapCompleted?.Invoke();

		// await Task.Delay(500); // имитация дольше

		string sceneToLoad = GameBootstrap.RequestedScene ?? SceneName;

		// SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
		
		// _sceneLoaderService = ServiceLocator.Get<SceneLoaderService>();
		// _sceneLoaderService.Test();
		// SceneHelper.LoadScene(sceneToLoad, additive: true, setActive: true);
	}

	private void CollectServices()
	{
		foreach (var mono in _monoServices)
		{
			if (mono is IService service && !_allServices.Contains(service))
				_allServices.Add(service);
			else
				Debug.Log($"{mono.name} - Is not Service");
		}

		foreach (var so in _scriptableServices)
		{
			if (so is IService service && !_allServices.Contains(service))
				_allServices.Add(service);
			else
				Debug.Log($"{so.name} - Is not Service");
		}

		foreach (var type in _plainServiceTypes)
		{
			if (typeof(IService).IsAssignableFrom(type) && !type.IsAbstract)
			{
				var instance = (IService)Activator.CreateInstance(type);
				_allServices.Add(instance);
			}
			else
				Debug.Log($"{type} - Is not Service");
		}
	}

	private void RegisterServices()
	{
		foreach (var service in _allServices)
		{
			var type = service.GetType();
			ServiceLocator.Register(service, type);
		}
	}

	private async Task InitializeServices()
	{
		var tasks = new List<Task>();
		foreach (var service in _allServices)
		{
			tasks.Add(service.InitializeAsync());
		}
		await Task.WhenAll(tasks);
	}
}

public static class GameBootstrap
{
	public static string RequestedScene;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void OnGameStart()
	{
		string activeScene = SceneManager.GetActiveScene().name;

		if (activeScene != "BootstrapScene")
		{
			RequestedScene = activeScene;
			SceneManager.LoadScene("BootstrapScene");
		}
		else
		{
			RequestedScene = null;
		}
	}
}


