using System.Threading.Tasks;
using UnityEngine;
using Eflatun.SceneReference;

public class TestServiceMono : MonoBehaviour, IService
{
	[SerializeField] private SceneReference mySceneReference;

	private void Awake()
	{
		Debug.Log(mySceneReference.Name);
	}

	public async Task InitializeAsync()
	{
		Debug.Log("TestServiceMono init started...");
		await Task.Delay(500);
		Debug.Log("TestServiceMono  init finished!");
	}

	public void Test()
	{
		Debug.Log("TestServiceMono ");
	}
}
