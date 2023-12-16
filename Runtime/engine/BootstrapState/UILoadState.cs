using System.Collections.Generic;
using CofyUI;
using UnityEngine;

namespace CofyEngine
{
    public abstract class UILoadState : IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.UI;
        
        protected abstract Future<List<GameObject>> LoadAll();


        protected Future<GameObject> LoadUI(string path)
        {
            return AssetManager.instance
                .LoadAsset<GameObject>(string.Format("{0}/{1}", ConfigSO.inst.uiDirectory, path), AssetLoadOption.None);
        }

        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            var loadingScreen = LoadingScreen.instance;
            loadingScreen.SetGoActive(true);
            
            var loadFuture = LoadAll();

            loadingScreen.MonitorProgress(loadFuture, "loading UI");

            loadFuture.Then(_ =>
            {
                sm.GoToState(BootStateId.UGS);
            });
        }

        public void OnEndContext() { }
    }
}