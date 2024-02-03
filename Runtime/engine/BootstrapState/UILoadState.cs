using System.Collections.Generic;

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
            var loading = LoadingUIPanel.instance;
            
            var loadFuture = LoadAll();

            loading.MonitorProgress(loadFuture, "loading UI");

            loadFuture.Then(_ =>
            {
                sm.GoToState(BootStateId.UGS);
            });
        }

        public void OnEndContext() { }
    }
}