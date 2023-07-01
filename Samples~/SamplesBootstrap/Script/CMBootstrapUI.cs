using System.Collections.Generic;
using CofyEngine;
using CofyUI;
using UnityEngine;

public class CMBootstrapUI : BootstrapUI
{
    public override Future<List<GameObject>> LoadAll()
    {
        var uiRoot = UIRoot.Singleton;

        List<Future<GameObject>> promises = new List<Future<GameObject>>();

        // promises.Add(uiRoot.Bind<LoginUI>(LoadUIAssetAsync("LoginUI")));

        return promises.Sequence();
    }
}