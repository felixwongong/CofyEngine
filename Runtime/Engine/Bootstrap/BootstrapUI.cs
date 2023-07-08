using System.Collections;
using System.Collections.Generic;
using CofyEngine.Engine;
using CofyEngine.Engine.Util;
using CofyEngine.Engine.Util.StateMachine;
using CofyUI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

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
            Future<List<GameObject>> uiPromise;
            UIRoot.Singleton.Bind<LoadingScreen>(LoadUIAssetAsync("Loading/loading_panel"))
                .Then(future =>
                {
                    var loadingScreen = LoadingScreen.instance;
                    loadingScreen.SetGoActive(true);
                    uiPromise = LoadAll();

                    loadingScreen.MonitorProgress(uiPromise.promise);

                    uiPromise.Then(_ =>
                    {
                        FLog.Log("UI load finished.");
                        sm.GoToNextState<BootstrapUGS>();
                    });
                });
        }
    }
}