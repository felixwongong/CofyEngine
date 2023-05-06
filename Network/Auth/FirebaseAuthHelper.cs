using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Network.CloudSave;
using CM.Util.Singleton;
using cofydev.util;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace CM.Network.Auth
{
    public class FirebaseAuthHelper : SingleBehaviour<FirebaseAuthHelper>
    {
        public override bool destroyWithScene => false;
        private FirebaseApp app;
        private FirebaseAuth auth;

        public bool isFirebaseReady => app != null;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = FirebaseApp.DefaultInstance;
                    
#if UNITY_EDITOR
                    FirebaseFirestore.DefaultInstance.Settings.PersistenceEnabled = false;
#endif
                    auth = FirebaseAuth.GetAuth(app);
                    FLog.Log($"current user: {auth.CurrentUser.UserId} ");
                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    FLog.LogError(String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        public async Task HandleEmailSignIn(string email, string password)
        {
            await auth.FetchProvidersForEmailAsync(email).ContinueWith(async (authTask) =>
            {
                if (authTask.IsCanceled)
                {
                    FLog.Log("Provider fetch canceled.");
                }
                else if (authTask.IsFaulted)
                {
                    FLog.Log("Provider fetch encountered an error.");
                    FLog.Log(authTask.Exception.ToString());
                }
                else if (authTask.IsCompleted)
                {
                    FLog.Log("Email Providers:");
                    var providers = authTask.Result;
                    if(providers.ToArray().Length <= 0) {}
                    foreach (string provider in authTask.Result)
                    {
                        FLog.Log(provider);
                    }

                    if (authTask.Result.ToArray().Length <= 0)
                    {
                        await CreateEmailUser(email, password);
                    }
                    else
                        await SignInEmail(email, password);
                }
            });
        }

        public async Task SignInEmail(string email, string password)
        {
            await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    FLog.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    FLog.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;

                FLog.Log($"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
            });
        }

        public async Task CreateEmailUser(string email, string password)
        {
            await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
        }

        public async Task SignInFB(string token)
        {
            Credential credential =
                FacebookAuthProvider.GetCredential(token);
            FLog.Log($"credential {token}");
            await auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    FLog.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    FLog.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;
                FLog.Log($"FB User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
            });
        }

        public async Task SignInCustomToken(string token)
        {
            await auth.SignInWithCustomTokenAsync(token).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCustomTokenAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCustomTokenAsync encountered an error: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
        }

        public async void SignInAnonymously()
        {
            await auth.SignInAnonymouslyAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    FLog.LogError("SignInAnonymouslyAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    FLog.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;
                FLog.Log($"Anonymous User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
            });
        }

        async Task SignInWithOpenIdConnectAsync(string idProviderName, string idToken)
        {
            try
            {
                FLog.Log("Start open id sign in");
                await AuthenticationService.Instance.SignInWithOpenIdConnectAsync(idProviderName, idToken);
                FLog.Log("open id SignIn is successful.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                FLog.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                FLog.LogException(ex);
            }
        }

        public async Task DeleteCurrentAccount()
        {
            var currentUser = auth.CurrentUser;
            if (currentUser == null) return;

            try
            {
                await FirestoreWrapper.Singleton.RemoveUserCollection();
                await currentUser.DeleteAsync();
                FLog.Log("Firebase user deleted successfully.");
            }
            catch (Exception e)
            {
                FLog.LogException(e);
            }
        }
    }
}