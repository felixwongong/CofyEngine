using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string uiRootPath = "Assets/Prefab/UI";

    private void Awake()
    {
        var bootstrapUI = new BootstrapUI(uiRootPath);
        bootstrapUI.loadAll();
    }
}
