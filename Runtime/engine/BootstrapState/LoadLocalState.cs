using UnityEngine.UIElements;

namespace CofyEngine
{
    public class LoadLocalState: IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.LoadLocal;
        
        protected Future<VisualTreeAsset> LoadLocalUI(string path)
        {
            return AssetManager.instance
                .LoadAsset<VisualTreeAsset>(string.Format("{0}/{1}", ConfigSO.inst.uiDirectory, path));
        }
        
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            LoadLocalUI("UIPanel/LoadingUIPanel/loading_panel").OnSucceed(asset =>
            {
                UIManager.instance.Bind(new LoadingUIPanel(asset), BindingOption.Clone);
                LoadingUIPanel.instance.Show();
                sm.GoToState(BootStateId.AtlasLoad);
            });
        }

        public void OnEndContext()
        {
        }
    }
}