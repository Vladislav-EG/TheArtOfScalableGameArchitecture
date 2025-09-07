using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Если нужно для выхода, но для простоты используем Application.Quit()

public class PauseService : MonoBehaviour
{
	// Singleton instance
	public static PauseService Instance { get; private set; }

	// UI элементы: предполагаем, что у вас есть Canvas с Panel для паузы
	[SerializeField] private GameObject pausePanel; // Прикрепите ваш Pause Panel в инспекторе
	[SerializeField] private Button continueButton; // Кнопка "Продолжить"
	[SerializeField] private Button exitButton; // Кнопка "Выйти"

	// [SerializeField] private InputReader _inputReader;
	[SerializeField] private InputService _inputService;
	

	private bool isPaused = false;

	private void Awake()
	{
		// Реализация singleton: убедимся, что только один экземпляр
		if (Instance == null)
		{
			Instance = this;
			// DontDestroyOnLoad(gameObject); // Чтобы сервис не уничтожался при смене сцен
		}
		else
		{
			Destroy(gameObject);
		}

		// Настройка кнопок
		if (continueButton != null)
		{
			continueButton.onClick.AddListener(ResumeGame);
		}

		if (exitButton != null)
		{
			exitButton.onClick.AddListener(ExitGame);
		}

		// Изначально скрываем панель паузы
		if (pausePanel != null)
		{
			pausePanel.SetActive(false);
		}
	}

	/// <summary>
	/// Запустить паузу
	/// </summary>
	public void PauseGame()
	{
		if (!isPaused)
		{
			_inputService.EnableUI();

			isPaused = true;
			Time.timeScale = 0f; // Остановить время в игре
			if (pausePanel != null)
			{
				pausePanel.SetActive(true); // Показать панель паузы
			}
		}
	}

	/// <summary>
	/// Возобновить игру
	/// </summary>
	public void ResumeGame()
	{
		if (isPaused)
		{
			_inputService.EnableGameplay();

			isPaused = false;
			Time.timeScale = 1f; // Возобновить время
			if (pausePanel != null)
			{
				pausePanel.SetActive(false); // Скрыть панель
			}
		}
	}

	/// <summary>
	/// Выйти из игры
	/// </summary>
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