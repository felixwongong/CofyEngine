using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CM.Network.CloudSave;
using cofydev.util;
using Firebase.Auth;
using Mono.CSharp;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

namespace CM.Network.Auth
{
    public class AuthServiceProvider : BaseService
    {
        public bool hasSignInRecord => AuthenticationService.Instance.SessionTokenExists;

        public ESignInPlatform activePlatform;
        public string playerId => AuthenticationService.Instance.PlayerId;

        public override void Init()
        {
            SetupEvents();
            Debug.Log($"isExpired: {AuthenticationService.Instance.IsExpired}");
            Debug.Log($"Sign in record existed? {hasSignInRecord}");
        }

        private void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                // Shows how to get a playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            };

            AuthenticationService.Instance.SignInFailed += err => { Debug.LogError(err); };

            AuthenticationService.Instance.SignedOut += () => { Debug.Log("Player signed out."); };

            AuthenticationService.Instance.Expired += () =>
            {
                Debug.Log("Player session could not be refreshed and expired.");
            };
        }

        public async Task PerformSignIn(ESignInPlatform signInPlatform)
        {
            var cloudSave = GetComponent<CloudSaveServiceProvider>();
            switch (signInPlatform)
            {
                //TODO: Need handle directing Anonymous Sign-In to one of the Firebase Profile SignIn;
                case ESignInPlatform.ANONYMOUS:
                    await SignInAnonymouslyAsync();
                    var ids = AuthenticationService.Instance.PlayerInfo.Identities;
                    if (ids is { Count: > 0 })
                    {
                        var platform = GetPlatformFromId(ids[0]);
                        await SignInFirebase(platform);
                    }
                    else
                    {
                        var pid = AuthenticationService.Instance.PlayerId;
                        var email = $"{pid.Substring(0, 20)}@cofy.com";
                        var pw = pid.Substring(pid.Length - 20, 20);
                        await FirebaseAuthHelper.Singleton.HandleEmailSignIn(email, pw);
                    }
                    break;
                case ESignInPlatform.FACEBOOK:
                    string token = await SignInFacebookAsync();
                    FLog.Log(token);
                    await cloudSave.SaveOne(new Dictionary<string, object>()
                        { { SaveKey.TOKEN, token } });
                    await SignInFirebase(ESignInPlatform.FACEBOOK);
                    break;
                default:
                    print("Sign In option not provided");
                    break;
            }

            activePlatform = signInPlatform;
        }

        private async Task SignInFirebase(ESignInPlatform platform)
        {
            switch (platform)
            {
                case ESignInPlatform.ANONYMOUS:
                    break;
                case ESignInPlatform.FACEBOOK:
                    var token = await GetComponent<CloudSaveServiceProvider>().GetOne(SaveKey.TOKEN);
                    await FirebaseAuthHelper.Singleton.SignInFB(token);
                    break;
                default:
                    FLog.Log("Sign In option not provided");
                    break;
            }
        }

        private async Task<string> SignInFacebookAsync()
        {
            var fb = FacebookUtil.Singleton;
            fb.Login();
            while (string.IsNullOrEmpty(fb.Token))
            {
                await Task.Delay(100);
            }

            await AuthenticationService.Instance.SignInWithFacebookAsync(fb.Token);
            Debug.Log("FB signed in successfully");
            return fb.Token;
        }

        private async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");

                // Shows how to get the playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        public async Task DeleteCurAccountAsync()
        {
            await AuthenticationService.Instance.DeleteAccountAsync();
            await FirebaseAuthHelper.Singleton.DeleteCurrentAccount();
        }

        private ESignInPlatform GetPlatformFromId(Identity id)
        {
            switch (id.TypeId)
            {
                case "facebook.com":
                    return ESignInPlatform.FACEBOOK;
                default:
                    FLog.Log("Platform in id not found");
                    break;
            }

            throw new Exception("Platform in id not found");
        }
    }

    [Serializable]
    public enum ESignInPlatform
    {
        ANONYMOUS,
        FACEBOOK,
    }
}