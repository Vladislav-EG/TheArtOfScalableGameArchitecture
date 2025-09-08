using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapperMono : MonoBehaviour
{
	[SerializeField] private string SceneName = "SampleScene";

	[SerializeField] private List<MonoBehaviour> _monoServices = new List<MonoBehaviour>();

	[SerializeField] private List<ScriptableObject> _scriptableServices = new List<ScriptableObject>();

	private List<Type> _plainServiceTypes = new List<Type>
	{
		typeof(TestService),
		typeof(TestService2)

	};

	private List<IService> _allServices = new List<IService>();

	private async void Awake()
	{
		CollectServices();
		RegisterServices();
		await InitializeServices();

		Debug.Log("The loading of services is completed");

		SceneManager.LoadScene(SceneName, LoadSceneMode.Additive); // TODO: SceneLoaderService
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