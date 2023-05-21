using System;
using CM.Util.Singleton;
using cofydev.util;
using CofyUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CofyEngine.Engine.Game
{
    public class LevelManager : SingleBehaviour<LevelManager>
    {
        public override bool destroyWithScene => false;

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

            sceneLoadPromise.Succeed += _ =>
            {
                after?.Invoke(disposingScene, SceneManager.GetSceneByName(sceneName));
                SceneManager.UnloadSceneAsync(disposingScene);
                LoadingScreen.instance.EndMonitoring();
                FLog.Log($"{sceneName} load end");
            };
        }
    }
}