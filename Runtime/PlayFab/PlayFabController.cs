using System;
using CofyEngine.Engine.Util;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace CofyEngine.PlayFab
{
    public class PlayFabController: Instance<PlayFabController>
    {
        private Future<bool> _initLogin()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                //TODO: add localized exception
                return new Failure<bool>(new Exception("Network not reachable"));

            var _activeClient = new PlayFabClientInstanceAPI();

            var param = CreatePlayerParams();

            var request = new LoginWithCustomIDRequest();
            
            return new Success<bool>(true);
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