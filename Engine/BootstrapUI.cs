using System.Collections.Generic;
using CofyEngine;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BootstrapUI: MonoBehaviour
{
    [SerializeField] private string uiRootPath = "Assets/Prefab/UI";


    public virtual void LoadAll()
    {
    }

    public Promise<GameObject> SpawnAddressableUI(string path)
    {
        var handle = Addressables.InstantiateAsync($"{uiRootPath}/{path}.prefab");
        return handle.ToPromise().Then(_ => Addressables.Release(handle));
    }
}