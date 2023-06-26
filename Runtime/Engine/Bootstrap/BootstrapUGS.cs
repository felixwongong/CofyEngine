using CofyEngine.Engine;
using CofyEngine.UGS;
using Engine.Util;

namespace CofyEngine 
{
    public class BootstrapUGS : MonoInstance<BootstrapUGS>, IPromiseState
    {
#if UNITY_EDITOR
        public string devCustomId = "Tester";
#endif

        void IPromiseState.StartContext(IPromiseSM sm)
        {
            UGSController.instance.InitLogin().Then(success =>
            {
                if(success.result) sm.GoToNextState<TerminateState>();
            });
        }
    }
}