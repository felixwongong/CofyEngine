using CofyEngine.Engine;
using CofyUI;
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
            var future = UnityServices.InitializeAsync().ToPromise().future
                .Then(_ =>
                {
                    sm.GoToNextState<TerminateState>();

                });
            
            LoadingScreen.instance.MonitorProgress(future);
        }
    }
}