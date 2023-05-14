using System;
using CM.Util.Singleton;
using cofydev.util;
using CofyUI;
using UnityEngine.SceneManagement;

namespace CofyEngine.Engine.Game
{
    public class LevelManager : SingleBehaviour<LevelManager>
    {
        public override bool destroyWithScene => false;

        public void LoadLevel(string sceneName, Action<Scene> before = null, Action<Scene, Scene> after = null)
        {
            FLog.Log($"{sceneName} load start");

            Scene disposingScene = SceneManager.GetActiveScene();
            before?.Invoke(disposingScene);

            UIRoot.Singleton.DisableAllInstances();
            
            Promise<bool> sceneLoadPromise =
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToPromise();
            LoadingScreen.instance.MonitorProgress(sceneLoadPromise);

            sceneLoadPromise.Succeed += _ =>
            {
                after?.Invoke(disposingScene, SceneManager.GetSceneByName(sceneName));
                SceneManager.UnloadSceneAsync(disposingScene);
                FLog.Log($"{sceneName} load end");
            };
        }
    }
}