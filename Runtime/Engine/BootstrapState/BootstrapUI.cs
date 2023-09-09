using System;
using System.Collections.Generic;
using CofyEngine.Engine.Util;
using CofyUI;
using UnityEngine;

namespace CofyEngine
{
    public abstract class BootstrapUI : IPromiseState
    {
        protected abstract Future<List<GameObject>> LoadAll();

        protected Future<GameObject> LoadUIAssetAsync(string path)
        {
            return CofyAddressable.LoadAsset<GameObject>(AssetPath.UI, path);
        }

        private Future<GameObject> LoadLocalUI(string path)
        {
            var req = Resources.LoadAsync<GameObject>($"LocalUI/{path}");
            return req.Future().TryMap(_ => (GameObject)req.asset);
        }

        void IPromiseState.StartContext(IPromiseSM sm)
        {
            Future<List<GameObject>> loadFuture;
            UIRoot.Singleton.Bind<LoadingScreen>(LoadLocalUI("Loading/loading_panel"))
                .Then(future =>
                {
                    var loadingScreen = LoadingScreen.instance;
                    loadingScreen.SetGoActive(true);
                    loadFuture = LoadAll();

                    loadingScreen.MonitorProgress(loadFuture);

                    loadFuture.Then(_ =>
                    {
                        FLog.Log("UI load finished.");
                        sm.GoToNextState<BootstrapUGS>();
                    });
                });
        }
    }
}