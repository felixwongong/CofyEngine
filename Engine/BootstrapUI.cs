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

    public PromiseImpl<GameObject> SpawnAddressableUI(string path)
    {
        return
            Addressables.InstantiateAsync($"{uiRootPath}/{path}.prefab")
                .ToPromise();
    }
}