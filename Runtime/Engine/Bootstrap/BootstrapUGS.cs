using CofyEngine.Engine;
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
            var promise = UnityServices.InitializeAsync().ToPromise().future
                .Then(_ =>
                {
                });
        }
    }
}