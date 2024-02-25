using System.Collections.Generic;

namespace CofyEngine
{
    public class LoadLocalState: IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.LoadLocal;
        
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            var panelFuture = UIManager.instance.Bind(new LoadingUIPanel() ,"Assets/_New/UI/UIPanel/LoadingUIPanel/loading_panel.uxml", BindingOption.CreateInstance);
            panelFuture.OnSucceed(panel =>
            {
                panel.Show();
                sm.GoToState(BootStateId.AtlasLoad);
            });
        }

        public void OnEndContext()
        {
        }
    }
}