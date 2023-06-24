﻿using System.Collections;
using CofyEngine.Engine;
using CofyEngine.Engine.Util.StateMachine;
using CofyEngine.PlayFab;
using Engine.Util;

namespace CofyEngine 
{
    public class BootstrapPlayFab : MonoInstance<BootstrapPlayFab>, IPromiseState
    {
#if UNITY_EDITOR
        public string devCustomId = "Tester";
#endif

        void IPromiseState.StartContext(IPromiseSM sm)
        {
            PlayFabController.instance.InitLogin();
        }
    }
}