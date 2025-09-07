using UnityEngine;

public class BootstrapTest : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		// BootstrappedData.Instance.Test();
		
		Bootstrapper.Instance.allServices[0].Test();
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
