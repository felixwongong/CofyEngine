using System.Collections;
using System.Collections.Generic;
using cofydev.util;
using cofydev.util.StateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CofyUI
{
    public abstract class BootstrapUI : IStateContext
    {
        private string uiRootPath;

        public BootstrapUI(string uiRootPath)
        {
            this.uiRootPath = uiRootPath;
        }

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
                    uiPromise.Succeed += list => loadingFinished = true;
                });

            yield return new WaitUntil(() => loadingFinished == true);
            
        }
    }
}