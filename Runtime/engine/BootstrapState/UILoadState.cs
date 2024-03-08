using System.Collections.Generic;

namespace CofyEngine
{
    public abstract class UILoadState : BaseState<BootStateId>
    {
        public override BootStateId id => BootStateId.UI;
        
        protected abstract Future<List<UIPanel>> LoadAll();

        protected Future<UIPanel> BindUI(UIPanel panel, string uxmlPath, BindingOption option = BindingOption.None)
        {
            return UIManager.instance.Bind(panel, uxmlPath, option);
        }

        protected internal override void StartContext(IStateMachine<BootStateId> sm, object param)
        {
            var loading = LoadingUIPanel.instance;
            
            var loadFuture = LoadAll();

            loading.MonitorProgress(loadFuture, "loading UI");

            loadFuture.Then(_ =>
            {
                sm.GoToState(BootStateId.Login);
            });
        }
    }

}
