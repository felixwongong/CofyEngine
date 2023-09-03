using System;
using CofyEngine.Engine.Util;
using CofyUI;
using UnityEngine.SceneManagement;

namespace CofyEngine.Engine
{
    public class LevelManager : Instance<LevelManager>
    {
        public void LoadLevelFull(string sceneName, Action<Scene> before = null, Action<Scene, Scene> after = null)
        {
            FLog.Log($"{sceneName} load start");

            Scene disposingScene = SceneManager.GetActiveScene();
            before?.Invoke(disposingScene);

            UIRoot.Singleton.DisableAllInstances();

            // var aop = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            // aop.allowSceneActivation = true;
            //
            // Promise<AsyncOperation> sceneLoadPromise = aop.ToPromise();

            var sceneLoadFuture = CofyAddressable.LoadScene(sceneName, LoadSceneMode.Additive);
            
            LoadingScreen.instance.MonitorProgress(sceneLoadFuture);
            
            sceneLoadFuture.OnCompleted(sceneValidate =>
            {
                if (sceneValidate.hasException)
                {
                    FLog.LogException(sceneValidate.target.ex);    
                }
                else
                {
                    after?.Invoke(disposingScene, sceneValidate.target.result);
                    SceneManager.UnloadSceneAsync(disposingScene);
                    LoadingScreen.instance.EndMonitoring();
                    FLog.Log($"{sceneName} load end");
                }
            });
        }
    }
}