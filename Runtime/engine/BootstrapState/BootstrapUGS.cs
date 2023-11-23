using CofyUI;
using Unity.Services.Core;

namespace CofyEngine
{
    public class BootstrapUGS : IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.UGS;
        
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            var future = UnityServices.InitializeAsync().Future()
                .Then(_ =>
                {
                    sm.GoToState(BootStateId.Terminate);

                });
            
            LoadingScreen.instance.MonitorProgress(future);
        }

        public void OnEndContext() { }
    }
}