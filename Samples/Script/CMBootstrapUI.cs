using System.Collections.Generic;
using CM.UI;
using CofyEngine;
using CofyUI;
using UnityEngine;

namespace CM.Core
{
    public class CMBootstrapUI : BootstrapUI
    {
        public override Promise<List<GameObject>> LoadAll()
        {
            var uiRoot = UIRoot.Singleton;
            
            List<Promise<GameObject>> promises = new List<Promise<GameObject>>();

            promises.Add(uiRoot.Bind<LoginUI>(LoadUIAssetAsync("LoginUI")));

            return promises.Sequence();
        }
    }
}