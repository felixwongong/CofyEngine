using System;
using CM.Util.Singleton;
using CofyEngine.Engine.Util;
using CofyUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CofyEngine.Engine.Core
{
    public class LevelManager : Instance<LevelManager>
    {
        public void LoadLevelFull(string sceneName, Action<Scene> before = null, Action<Scene, Scene> after = null)
        {
            FLog.Log($"{sceneName} load start");

            Scene disposingScene = SceneManager.GetActiveScene();
            before?.Invoke(disposingScene);

            UIRoot.Singleton.DisableAllInstances();

            var aop = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            aop.allowSceneActivation = true;

            Promise<AsyncOperation> sceneLoadPromise = aop.ToPromise();

            LoadingScreen.instance.MonitorProgress(sceneLoadPromise);

            sceneLoadPromise.Completed += opValidate =>
            {
                if (opValidate.hasException)
                {
                    FLog.LogException(opValidate.target.ex);    
                }
                else
                {
                    after?.Invoke(disposingScene, SceneManager.GetSceneByName(sceneName));
                    SceneManager.UnloadSceneAsync(disposingScene);
                    LoadingScreen.instance.EndMonitoring();
                    FLog.Log($"{sceneName} load end");
                }
            };
        }
    }
}