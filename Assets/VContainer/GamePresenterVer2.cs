using UnityEngine;
using VContainer;

public class GamePresenterVer2 : MonoBehaviour
{
	private HelloWorldService _helloWorldService;

	[Inject]
	public void Construct(HelloWorldService helloWorldService)
	{
		_helloWorldService = helloWorldService;
	}

	public void Start()
	{
		Debug.Log("ASDSA");
		_helloWorldService.Hello();
	}
}
