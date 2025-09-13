using System.Threading.Tasks;
using UnityEngine;
using Eflatun.SceneReference;

public class TestServiceMono : MonoBehaviour, IService
{
	// [SerializeField] private SceneReference mySceneReference;
	
	private SceneLoaderService _sceneLoaderService;

	private void Awake()
	{
		// Debug.Log(mySceneReference.Name);
		_sceneLoaderService = ServiceLocator.Get<SceneLoaderService>();
		
	}

	public async Task InitializeAsync()
	{
		// Debug.Log("TestServiceMono init started...");
		await Task.Delay(500);
		// Debug.Log("TestServiceMono  init finished!");
		// _sceneLoaderService = ServiceLocator.Get<SceneLoaderService>();
	}

	public void Test()
	{
		Debug.Log("TestServiceMono ");
	}
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			_sceneLoaderService.EndLevelEvent("LevelTwo");
		}
	}
}
