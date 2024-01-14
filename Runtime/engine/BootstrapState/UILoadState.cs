using System.Collections.Generic;
using CofyUI;
using UnityEngine;

namespace CofyEngine
{
    public abstract class UILoadState : IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.UI;
        
        protected abstract Future<List<UIPanel>> LoadAll();

        protected Future<T> BindUI<T>(T panel, BindingOption option = BindingOption.None) where T: UIPanel
        {
            return UIManager.instance.Bind(panel, option);
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