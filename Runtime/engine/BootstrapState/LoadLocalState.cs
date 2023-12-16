using CofyUI;
using UnityEngine;

namespace CofyEngine
{
    public class LoadLocalState: IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.LoadLocal;
        
        protected Future<GameObject> LoadLocalUI(string path)
        {
            return AssetManager.instance
                .LoadAsset<GameObject>(string.Format("{0}/{1}", ConfigSO.inst.localPath, path), AssetLoadOption.ForceLoadLocal);
        }
        
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            LoadLocalUI("UIRoot").OnSucceed(uiRoot =>
            {
                UIRoot.instance.Bind<LoadingScreen>(LoadLocalUI("loading_panel"))
                    .OnSucceed(_ => sm.GoToState(BootStateId.AtlasLoad));
            });
        }

        public void OnEndContext()
        {
        }
    }
}