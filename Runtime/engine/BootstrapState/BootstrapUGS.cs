using CofyUI;
using Unity.Services.Core;

namespace CofyEngine
{
    public class BootstrapUGS : IPromiseState
    {
        void IPromiseState.StartContext(IPromiseSM sm, object param)
        {
            var future = UnityServices.InitializeAsync().Future()
                .Then(_ =>
                {
                    sm.GoToState<TerminateState>();

                });
            
            LoadingScreen.instance.MonitorProgress(future);
        }

        void IPromiseState.OnEndContext() { }
    }
}