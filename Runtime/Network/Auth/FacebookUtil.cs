using System.Collections.Generic;
using CM.Util.Singleton;
using UnityEngine;

// Other needed dependencies
using Facebook.Unity;
using UnityEngine.EventSystems;

namespace CM.Network.Auth
{
    public class FacebookUtil : SingleBehaviour<FacebookUtil>
    {
        public override bool destroyWithScene => false;
        public string Token;
        public string Error;

        // Awake function from Unity's MonoBehavior
        protected override void Awake()
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }

        void InitCallback()
        {
            //Fix FB eventsystem ddol bug
            var eventSystems = FindObjectsOfType<EventSystem>();
            foreach (var eventSystem in eventSystems)
            {
                var tmp = new GameObject("Event System Container");
                eventSystem.gameObject.transform.parent = tmp.transform;
            }
            //

            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        public void Login()
        {
            // Define the permissions
            var perms = new List<string>() { "public_profile", "email" };

            FB.LogInWithReadPermissions(perms, result =>
            {
                if (FB.IsLoggedIn)
                {
                    Token = AccessToken.CurrentAccessToken.TokenString;
                    Debug.Log($"Facebook Login token: {Token}");
                }
                else
                {
                    Error = "User cancelled login";
                    Debug.Log("[Facebook Login] User cancelled login");
                }
            });
        }
    }
}