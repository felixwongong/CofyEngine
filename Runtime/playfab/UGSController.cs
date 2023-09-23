using System;
using CofyEngine.Engine.Util;
using UnityEngine;

namespace CofyEngine.UGS
{
    public class UGSController: Instance<UGSController>
    {
        public Future<bool> InitLogin()
        {
            //TODO: add recovery
            return _InitLogin();
        }

        private Future<bool> _InitLogin()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                //TODO: add localized exception
                return Future<bool>.failure(new Exception("Network not reachable."));

            return null;
        }
    }
}