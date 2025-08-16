using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
	public void InstallBindings(ContainerBuilder containerBuilder)
	{
		containerBuilder.AddSingleton("Hello RefleX");
	}
}
