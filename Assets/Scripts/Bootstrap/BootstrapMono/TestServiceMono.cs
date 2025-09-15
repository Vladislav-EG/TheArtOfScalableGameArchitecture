using System.Threading.Tasks;
using UnityEngine;

public class TestServiceMono : MonoBehaviour, IService
{
	// [SerializeField] private SceneReference mySceneReference;
	
	private SceneLoaderService _sceneLoaderService;
	
	private bool _test = true;
	private bool _test2 = true;

	private void Awake()
	{
		// Debug.Log(mySceneReference.Name);
		// _sceneLoaderService = ServiceLocator.Get<SceneLoaderService>();
		
	}

	public async Task InitializeAsync()
	{
		// Debug.Log("TestServiceMono init started...");
		// await Task.Delay(500);
		// Debug.Log("TestServiceMono  init finished!");
		_sceneLoaderService = ServiceLocator.Get<SceneLoaderService>();
		
		await Task.CompletedTask; 
	}

	public void Test()
	{
		Debug.Log("TestServiceMono ");
	}
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I) && _test)
		{
			// _sceneLoaderService.EndLevelEvent("LevelTwo");
			_sceneLoaderService.SetNextLevel();
			_test = false;
			_test2 = true;
			
		}
		else if (Input.GetKeyDown(KeyCode.O) && _test2)
		{
			// _sceneLoaderService.EndLevelEvent("LevelOne");
			_sceneLoaderService.SetNextLevel("LevelOne");
			_test2 = false;
			_test = true;
		}
	}
}
