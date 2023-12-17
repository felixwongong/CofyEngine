using System;
using Unity.Services.Core;

namespace CofyEngine.UGS
{
    public class UGSController
    {
        public static Future<bool> InitService()
        {
            if (NetworkReachability.reachable)
            {
                return Future<bool>.failure(new Exception("Network not reachable."));
            }
            else
            {
                return UnityServices.InitializeAsync().Future();
            }
        }

        public static Future<bool> InitLogin()
        {
            //TODO: add recovery
            return _InitLogin();
        }

        private static Future<bool> _InitLogin()
        {
            if (NetworkReachability.reachable)
                return Future<bool>.failure(new Exception("Network not reachable."));
            else
            {
                //Add Unity auth
                return UnityServices.InitializeAsync().Future();
            }
        }
    }
}