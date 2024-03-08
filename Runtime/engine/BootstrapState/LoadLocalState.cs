using System.Collections.Generic;

namespace CofyEngine
{
    public class LoadLocalState: BaseState<BootStateId>
    {
        public override BootStateId id => BootStateId.LoadLocal;

        protected internal override void StartContext(IStateMachine<BootStateId> sm, object param)
        {
            var panelFuture = UIManager.instance.Bind(new LoadingUIPanel() ,"Assets/_New/UI/UIPanel/LoadingUIPanel/loading_panel.uxml", BindingOption.CreateInstance);
            panelFuture.OnSucceed(panel =>
            {
                panel.Show();
                sm.GoToState(BootStateId.AtlasLoad);
            });
        }
    }
}