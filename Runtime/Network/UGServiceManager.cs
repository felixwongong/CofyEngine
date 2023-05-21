using CM.Util.Singleton;
using Unity.Services.Core;
using UnityEngine;

namespace CM.Network
{
    public class UGServiceManager : SingleBehaviour<UGServiceManager>
    {
        public override bool destroyWithScene => false;

        private async void Start()
        {
            var options = new InitializationOptions();

            await UnityServices.InitializeAsync(options);
            Debug.Log("UGS State: " + UnityServices.State);

            foreach (var service in GetComponents<BaseService>()) service.Init();
        }
    }
}