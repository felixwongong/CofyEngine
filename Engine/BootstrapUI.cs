using cofydev.util;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BootstrapUI
{
    private string uiRootPath;
    
    public BootstrapUI(string uiRootPath)
    {
        this.uiRootPath = uiRootPath;
    }

    public void loadAll()
    {
        Promise<GameObject> promise = LoadAddressableUI("LoginUI");
    }
    
    public Promise<GameObject> LoadAddressableUI(string path)
    {
        return Addressables.LoadAssetAsync<GameObject>($"{uiRootPath}/{path}.prefab").ToPromise();
    }
}