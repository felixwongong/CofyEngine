using System;
using System.Collections.Generic;
using CofyEngine.Engine.Util;
using CofyUI;
using UnityEngine.SceneManagement;

namespace CofyEngine
{
    public class LevelManager : Instance<LevelManager>
    {
        private List<string> persistentScenes;

        public void SetPersistent(List<string> scene)
        {
            this.persistentScenes = scene;
        }
        
        public void LoadLevel(string sceneName, bool additive = false, Action<Scene> before = null, Action<Scene, Scene> after = null)
        {
            FLog.Log($"{sceneName} load start");

            Scene disposingScene = SceneManager.GetActiveScene();
            before?.Invoke(disposingScene);

            UIRoot.instance.DisableAllUI();

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
                    if(!additive && !persistentScenes.Contains(disposingScene.name)) 
                        SceneManager.UnloadSceneAsync(disposingScene);
                    LoadingScreen.instance.EndMonitoring();
                    FLog.Log($"{sceneName} load end");
                }
            });
        }
    }
}