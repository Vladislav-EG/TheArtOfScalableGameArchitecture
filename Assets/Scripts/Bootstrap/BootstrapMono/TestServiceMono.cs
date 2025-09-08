using System.Threading.Tasks;
using UnityEngine;

public class TestServiceMono : MonoBehaviour, IService
{
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
