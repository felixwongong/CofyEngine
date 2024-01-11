using System.IO;
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
                LoadingUIPanel loadingUIPanel = new LoadingUIPanel(asset);
                UIManager.instance.Bind(loadingUIPanel, BindingOption.Clone);
                loadingUIPanel.Show();
            });
        }

        public void OnEndContext()
        {
        }
    }
}