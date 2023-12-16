using System;
using System.Collections.Generic;
using CofyEngine.Core;
using CofyEngine.Util;
using CofyUI;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CofyEngine
{
    public class LevelManager : Instance<LevelManager>
    {
        private List<string> persistentScenes;
        private Dictionary<string, SceneInstance> _sceneInstances = new();
        
        public void SetPersistent(List<string> scene)
        {
            this.persistentScenes = scene;
        }

        public void LoadLevel(string sceneName, bool additive = false, Action<Scene> before = null,
            Action<Scene, Scene> after = null)
        {
            MainThreadExecutor.instance.QueueAction(() => LoadLevelImpl(sceneName, additive, before, after));
        }
        
        private void LoadLevelImpl(string sceneName, bool additive = false, Action<Scene> before = null, Action<Scene, Scene> after = null)
        {
            FLog.Log($"{sceneName} load start");

            Scene disposingScene = SceneManager.GetActiveScene();
            before?.Invoke(disposingScene);

            UIRoot.instance.DisableAllUI();
            
            var sceneLoadFuture = CofyAddressable.LoadScene(ConfigSO.inst.sceneDirectory.concatPath(sceneName), LoadSceneMode.Additive);
            
            LoadingScreen.instance.MonitorProgress(sceneLoadFuture);
            
            sceneLoadFuture.OnCompleted(sceneValidate =>
            {
                if (sceneValidate.hasException)
                {
                    FLog.LogException(sceneValidate.target.ex);    
                }
                else
                {
                    var loadedInst = sceneValidate.target.result;
                    _sceneInstances.TryAdd(sceneName, loadedInst);
                    after?.Invoke(disposingScene, loadedInst.Scene);
                    if (!additive && !persistentScenes.Contains(disposingScene.name))
                    {
                        CofyAddressable.UnloadScene(_sceneInstances[disposingScene.name]);
                    }
                    LoadingScreen.instance.EndMonitoring();
                    FLog.Log($"{sceneName} load end");
                }
            });
        }
    }
}