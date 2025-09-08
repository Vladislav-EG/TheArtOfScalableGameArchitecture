using System.Threading.Tasks;
using UnityEngine;

public class TestServiceMono2 : MonoBehaviour, IService
{
	public async Task InitializeAsync()
	{
		Debug.Log("TestServiceMono2 init started...");
		await Task.Delay(1500); // имитация дольше
		Debug.Log("TestServiceMono2 init finished!");
	}

	public void Test()
	{
		Debug.Log("TestServiceMono2");
	}
}
