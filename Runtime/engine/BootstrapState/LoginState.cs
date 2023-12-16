using CofyEngine.UGS;
using CofyUI;

namespace CofyEngine
{
    public class LoginState : IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.UGS;
        
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            var initFuture = UGSController.InitService();
            
            initFuture.OnSucceed(_ =>
            {
                sm.GoToState(BootStateId.Terminate);
            });
            
            LoadingScreen.instance.MonitorProgress(initFuture, "connecting to service");
        }

        public void OnEndContext() { }
    }
}