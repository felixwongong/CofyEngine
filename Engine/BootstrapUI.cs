using System.Collections.Generic;
using cofydev.util;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BootstrapUI
{
    private string uiRootPath;
    
    //State
    private List<AsyncOperationHandle> handles;

    public BootstrapUI(string uiRootPath)
    {
        this.uiRootPath = uiRootPath;
        handles = new List<AsyncOperationHandle>();
    }

    public void loadAll()
    {
        Promise<GameObject> promise = LoadAddressableUI("LoginUI");
        
        promise.Succeed += result => FLog.Log(result.name);
    }
    
    public Promise<GameObject> LoadAddressableUI(string path)
    {
        return Addressables.LoadAssetAsync<GameObject>($"{uiRootPath}/{path}.prefab").ToPromise();
    }
}