using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
	// [SerializeField] интерфейс не может быть 
	// private IService testService = new TestService();
	
	public static Bootstrapper Instance;

	[SerializeField] private string SceneName = "SampleScene";

	public List<IService> allServices = new List<IService>() { new TestService(), new TestService2() };

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
		
		// allServices.Add(testService);

		foreach (IService service in allServices)
		{
			service.Initialize();
		}

		// DontDestroyOnLoad(gameObject);

		SceneManager.LoadScene(SceneName);
	}
}

public interface IService
{
	public void Initialize();
	public void Test();
}

public class TestService : IService
{
	public void Initialize()
	{
		Debug.Log("TestService Initialize");
	}
	
	public void Test()
	{
		Debug.Log("TestService");
	}
}

public class TestService2 : IService
{
	public void Initialize()
	{
		Debug.Log("TestService2 Initialize");
	}
	
		public void Test()
	{
		Debug.Log("TestService2");
	}
}