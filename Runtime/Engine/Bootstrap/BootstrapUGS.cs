using CofyEngine.Engine;
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
        }
    }
}