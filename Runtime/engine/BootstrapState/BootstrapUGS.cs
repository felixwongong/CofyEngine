using CofyEngine.UGS;
using CofyUI;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace CofyEngine
{
    public class BootstrapUGS : IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.UGS;
        
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            var initFuture = UGSController.InitService();
            
            initFuture.OnSucceed(_ =>
            {
                sm.GoToState(BootStateId.Terminate);
            });
            
            LoadingScreen.instance.MonitorProgress(initFuture);
        }

        public void OnEndContext() { }
    }
}