using System;
using CofyEngine.Engine;
using Engine.Util;
using Unity.Services.Core;

namespace CofyEngine
{
    public class BootstrapUGS : MonoInstance<BootstrapUGS>, IPromiseState
    {
#if UNITY_EDITOR
        public string devCustomId = "Tester";
#endif

        void IPromiseState.StartContext(IPromiseSM sm)
        {
            UnityServices.InitializeAsync().ToPromise().future
                .Then(_ =>
                {
                    FLog.Log("UGS Initialized");
                    sm.GoToNextState<TerminateState>();
                });
        }
    }
}