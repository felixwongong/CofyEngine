using System.Collections;
using CofyEngine.Runtime.Engine.Util.StateMachine;
using CofyUI;
using Unity.Services.Core;
using UnityEngine;

namespace CofyEngine 
{
    public class BootstrapUGS : MonoBehaviour, IStateContext
    {
        public IEnumerator StartContext(IStateMachine sm)
        {
            bool initialized = false;

            var options = new InitializationOptions();

            var promise = UnityServices.InitializeAsync(options).ToPromise();

            LoadingScreen.instance.MonitorProgress(promise);
            
            promise.Succeed += b => { initialized = true; };

            promise.Failed += failure => { FLog.LogException(failure.ex); };
            
            yield return new WaitUntil(() => initialized);
        }
    }
}