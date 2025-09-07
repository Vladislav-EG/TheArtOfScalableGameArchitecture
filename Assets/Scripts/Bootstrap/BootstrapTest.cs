using UnityEngine;

public class BootstrapTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BootstrappedData.Instance.Test();
        // LazyBootstrappedData.Instance.Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
