using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; // Если нужно для выхода, но для простоты используем Application.Quit()

public class PauseService : MonoBehaviour, IService
{
	private InputService _inputService;

	private bool isPaused = false;

	public async Task InitializeAsync()
	{
		_inputService = ServiceLocator.Get<InputService>();
	
		await Task.CompletedTask; 
	}

	public void PauseGame()
	{
		if (!isPaused)
		{
			_inputService.EnableUI();

			isPaused = true;
			Time.timeScale = 0f; 
		}
	}

	public void ResumeGame()
	{
		if (isPaused)
		{
			_inputService.EnableGameplay();

			isPaused = false;
			Time.timeScale = 1f;
		}
	}

	public void ExitGame()
	{
		// В редакторе Unity это остановит play mode, в билде выйдет из приложения
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

}