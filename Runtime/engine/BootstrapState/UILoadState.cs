﻿using System.Collections.Generic;
using CofyUI;
using UnityEngine;

namespace CofyEngine
{
    public abstract class UILoadState : IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.UI;
        
        protected abstract Future<List<GameObject>> LoadAll();

        protected Future<GameObject> LoadLocalUI(string path)
        {
            return AssetManager.instance
                .LoadAsset<GameObject>(string.Format("{0}/{1}", ConfigSO.inst.localPath, path), AssetLoadOption.ForceLoadLocal);
        }

        protected Future<GameObject> LoadUI(string path)
        {
            return AssetManager.instance
                .LoadAsset<GameObject>(string.Format("{0}/{1}", ConfigSO.inst.uiDirectory, path), AssetLoadOption.None);
        }

        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            Future<List<GameObject>> loadFuture;

            LoadLocalUI("UIRoot").OnSucceed(uiRoot =>
                {
                    UIRoot.instance.Bind<LoadingScreen>(LoadLocalUI("loading_panel"))
                        .Then(future =>
                        {
                            var loadingScreen = LoadingScreen.instance;
                            loadingScreen.SetGoActive(true);
                            loadFuture = LoadAll();

                            loadingScreen.MonitorProgress(loadFuture, "loading UI");

                            loadFuture.Then(_ =>
                            {
                                FLog.Log("UI load finished.");
                                sm.GoToState(BootStateId.UGS);
                            });
                        });
                });
        }

        public void OnEndContext() { }
    }
}