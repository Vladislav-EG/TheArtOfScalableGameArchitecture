using UnityEngine;

/* SceneLoaderService

1. Скачать и установить SceneReference
2. Создать сериализуемые: список уровней и для каждого уровня список сцен
3. Создать возможность выбирать в инспекторе сцену, которая будет активной сценой для уровня
4. Важно сделать так чтобы при загрузке аддативных сцены, сцены которых нет выгружались, а сцены которые были оставались

   */


public static class SceneHelper
{
	public static void LoadScene(string s, bool additive = false, bool setActive = false)
	{
		if (s == null)
		{
			s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene(
			s, additive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : 0);

		if (setActive)
		{
			// to mark it active we have to wait a frame for it to load.
			// Get the CallAfterDelay code at https://gist.github.com/kurtdekker/0da9a9721c15bd3af1d2ced0a367e24e
			CallAfterDelay.Create(0, () =>
			{
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(
					UnityEngine.SceneManagement.SceneManager.GetSceneByName(s));
			});
		}
	}

	public static void UnloadScene(string s)
	{
		UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s);
	}
}

public class CallAfterDelay : MonoBehaviour
{
	float delay;
	System.Action action;

	// Will never call this frame, always the next frame at the earliest
	public static CallAfterDelay Create(float delay, System.Action action)
	{
		CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
		cad.delay = delay;
		cad.action = action;
		return cad;
	}

	float age;

	void Update()
	{
		if (age > delay)
		{
			action();
			Destroy(gameObject);
		}
	}
	void LateUpdate()
	{
		age += Time.deltaTime;
	}
}

