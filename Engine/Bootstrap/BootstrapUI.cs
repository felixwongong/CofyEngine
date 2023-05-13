using System.Collections;
using System.Collections.Generic;
using cofydev.util;
using cofydev.util.StateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

namespace CofyUI
{
    public abstract class BootstrapUI : MonoBehaviour, IStateContext
    {
        [SerializeField] private string uiRootPath = "Assets/Prefab/UI";

        public abstract Promise<List<GameObject>> LoadAll();

        public Promise<GameObject> LoadUIAssetAsync(string path)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>($"{uiRootPath}/{path}.prefab");
            return handle.ToPromise();
        }

        public IEnumerator StartContext(IStateMachine sm)
        {
            bool loadingFinished = false;
            Promise<List<GameObject>> uiPromise;
            UIRoot.Singleton.Bind<LoadingScreen>(LoadUIAssetAsync("LoadingScreen"))
                .Then(future =>
                {
                    var loadingScreen = LoadingScreen.instance;
                    loadingScreen.SetGoActive(true);
                    uiPromise = LoadAll();

                    loadingScreen.MonitorProgress(uiPromise);

                    uiPromise.Succeed += list => loadingFinished = true;
                });

            yield return new WaitUntil(() => loadingFinished == true);
        }
    }
}