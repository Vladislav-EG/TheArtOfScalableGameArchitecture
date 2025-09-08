using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
	// public static Bootstrapper Instance;

	// [SerializeField] private string SceneName = "SampleScene";

	// public List<IService> allServices = new List<IService>()
	// {
	// 	new TestService(),
	// 	new TestService2()
	// };

	// private async void Awake()
	// {
	// 	if (Instance == null)
	// 	{
	// 		Instance = this;
	// 		DontDestroyOnLoad(gameObject);
	// 	}
	// 	else
	// 	{
	// 		Destroy(gameObject);
	// 		return;
	// 	}

	// 	// await InitializeServices();
	// 	await InitializeServicesServiceLocator();
	// 	// SceneManager.LoadScene(SceneName);
	// 	SceneManager.LoadScene(SceneName, LoadSceneMode.Additive); // TODO SceneLoaderService
	// }

	// // private async Task InitializeServices()
	// // {
	// // 	foreach (IService service in allServices)
	// // 	{
	// // 		await service.InitializeAsync();
	// // 	}
	// // }

	// private async Task InitializeServices()
	// {
	// 	var tasks = new List<Task>();

	// 	foreach (IService service in allServices)
	// 	{
	// 		tasks.Add(service.InitializeAsync());
	// 	}

	// 	await Task.WhenAll(tasks);
	// }

	// private async Task InitializeServicesServiceLocator()
	// {
	// 	var tasks = new List<Task>();

	// 	ServiceLocator.Register(new TestService());
	// 	ServiceLocator.Register(new TestService2());


	// 	tasks.Add(ServiceLocator.Get<TestService>().InitializeAsync());
	// 	tasks.Add(ServiceLocator.Get<TestService2>().InitializeAsync());


	// 	await Task.WhenAll(tasks);
	// }
}

public interface IService
{
	Task InitializeAsync();
}

public class TestService : IService
{
	public async Task InitializeAsync()
	{
		Debug.Log("TestService init started...");
		await Task.Delay(100);
		Debug.Log("TestService init finished!");
	}

	public void Test()
	{
		Debug.Log("TestService");
	}
}

public class TestService2 : IService
{
	public async Task InitializeAsync()
	{
		Debug.Log("TestService2 init started...");
		await Task.Delay(300); // имитация дольше
		Debug.Log("TestService2 init finished!");
	}

	public void Test()
	{
		Debug.Log("TestService2");
	}
}

