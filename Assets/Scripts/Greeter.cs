using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

public class Greeter : MonoBehaviour
{
	[Inject] private readonly IEnumerable<string> _strings;
	
	private HelloWorldService _helloWorldService;
	private HelloWorldServiceMono _helloWorldServiceMono;
	
	[Inject]
	private void Contruct(HelloWorldService helloWorldService, HelloWorldServiceMono helloWorldServiceMono)
	{
		_helloWorldService = helloWorldService;
		_helloWorldServiceMono = helloWorldServiceMono;
	}

	private void Start()
	{
		Debug.Log(string.Join(" ", _strings));
		
		_helloWorldService.Hello();
		_helloWorldServiceMono.Hello();
	}
}