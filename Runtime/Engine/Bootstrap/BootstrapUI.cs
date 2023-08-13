using System.Collections.Generic;
using CofyEngine.Engine;
using CofyEngine.Engine.Util;
using CofyUI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CofyEngine
{
    public abstract class BootstrapUI : MonoBehaviour, IPromiseState
    {
        [SerializeField] private string uiRootPath = "Assets/Prefab/UI";

        public abstract Future<List<GameObject>> LoadAll();

        public Future<GameObject> LoadUIAssetAsync(string path)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>($"{uiRootPath}/{path}.prefab");
            return handle.ToPromise().future;
        }

        void IPromiseState.StartContext(IPromiseSM sm)
        {
            Future<List<GameObject>> loadFuture;
            UIRoot.Singleton.Bind<LoadingScreen>(LoadUIAssetAsync("Loading/loading_panel"))
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