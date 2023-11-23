using System.Collections.Generic;
using CofyEngine.Util;
using CofyUI;
using UnityEngine;

namespace CofyEngine
{
    public abstract class BootstrapUI : IPromiseState<BootStateId>
    {
        protected abstract Future<List<GameObject>> LoadAll();

        protected Future<GameObject> LoadUIAssetAsync(string path)
        {
            return CofyAddressable.LoadAsset<GameObject>(PathResolver.GetAsset(AssetPath.UI, path));
        }

        private Future<GameObject> LoadLocalUI(string path)
        {
            var req = Resources.LoadAsync<GameObject>($"LocalUI/{path}");
            return req.Future().TryMap(_ => (GameObject)req.asset);
        }

        public BootStateId id { get; }
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            Future<List<GameObject>> loadFuture;

            LoadLocalUI("UIRoot").OnSucceed(uiRoot =>
            {
                Object.Instantiate(uiRoot);
                UIRoot.instance.Bind<LoadingScreen>(LoadLocalUI("Loading/loading_panel"))
                    .Then(future =>
                    {
                        var loadingScreen = LoadingScreen.instance;
                        loadingScreen.SetGoActive(true);
                        loadFuture = LoadAll();

                        loadingScreen.MonitorProgress(loadFuture);

                        loadFuture.Then(_ =>
                        {
                            FLog.Log("UI load finished.");
                            sm.GoToState(BootStateId.UGS);
                        });
                    });
            });
        }

        public void OnEndContext() { }
    }
}