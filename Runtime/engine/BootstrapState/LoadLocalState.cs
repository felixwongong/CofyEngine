namespace CofyEngine
{
    public class LoadLocalState: IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.LoadLocal;
        
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            UIManager.instance.Bind(new LoadingUIPanel() ,"Assets/_New/UI/UIPanel/LoadingUIPanel/loading_panel.uxml", BindingOption.CreateInstance);
            LoadingUIPanel.instance.Show();
        }

        public void OnEndContext()
        {
        }
    }
}