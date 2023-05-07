using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<BootstrapUI>().LoadAll();
    }
}
