using Reflex.Core;
using UnityEngine;

public class SampleSceneIntaller : MonoBehaviour, IInstaller
{
	[SerializeField] private HelloWorldServiceMono _helloWorldServiceMono;
	
	public void InstallBindings(ContainerBuilder containerBuilder)
	{
		containerBuilder.AddSingleton(_helloWorldServiceMono);	
	}
}