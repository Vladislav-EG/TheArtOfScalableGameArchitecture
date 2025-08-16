using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
	// [SerializeField] private HelloWorldServiceMono _helloWorldServiceMono;
	
	public void InstallBindings(ContainerBuilder containerBuilder)
	{
		containerBuilder.AddSingleton("Hello RefleX");
		containerBuilder.AddSingleton(new HelloWorldService());
		// containerBuilder.AddSingleton(_helloWorldServiceMono);
	}
}

public class HelloWorldService
{
	public void Hello()
	{
		Debug.Log("Hello WorldService Reflex");
	}
}
