using CofyEngine.Engine.Util;
using CofyUI;
using Unity.Services.Core;

namespace CofyEngine
{
    public class BootstrapUGS : IPromiseState
    {
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