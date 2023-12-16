using System;
using Unity.Services.Core;
using UnityEngine;

namespace CofyEngine.UGS
{
    public class UGSController
    {
        public static Future<bool> InitService()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                //TODO: Add confirmation panel for offline checking
                return Future<bool>.success(false);
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
            if (Application.internetReachability == NetworkReachability.NotReachable)
                return Future<bool>.failure(new Exception("Network not reachable."));
            else
            {
                //Add Unity auth
                return UnityServices.InitializeAsync().Future();
            }
        }
    }
}