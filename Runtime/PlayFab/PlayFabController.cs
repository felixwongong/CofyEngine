using System;
using CofyEngine.Engine.Util;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace CofyEngine.PlayFab
{
    public class PlayFabController: Instance<PlayFabController>
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
                return Future<bool>.failure(new Exception("Network not reachable"));

            var _activeClient = new PlayFabClientInstanceAPI();

            return PlayFabAuth.LoginWithCustomID(_activeClient, BootstrapPlayFab.instance.devCustomId).TryMap((_ => true)).future;
        }

        private GetPlayerCombinedInfoRequestParams CreatePlayerParams()
        {
            return  new GetPlayerCombinedInfoRequestParams()
            {
                GetCharacterInventories = false,
                GetCharacterList = false,
                GetPlayerProfile = false,
                GetPlayerStatistics = false,
                GetTitleData = false,
                GetUserData = false,
                GetUserInventory = false,
                GetUserAccountInfo = false,
                GetUserVirtualCurrency = false,
                GetUserReadOnlyData = false,
            };
        }
    }
}