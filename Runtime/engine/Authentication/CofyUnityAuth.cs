using System;
using CofyEngine.Util;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace CofyEngine
{
    public class CofyUnityAuth: Instance<CofyUnityAuth>
    {
        public static Func<Future<bool>, Exception> showException;
        
        private Future<bool> serviceInitFuture;
        
        public Future<bool> Init(object options = null)
        {
            serviceInitFuture = UnityServices.InitializeAsync(options as InitializationOptions).Future();
            return serviceInitFuture;
        }

        public Future<bool> SignInAnonymous(object options = null)
        {
            return serviceInitFuture.Then(_ => _SignInAnonymous(options));
        }
        
        private Future<bool> _SignInAnonymous(object options = null)
        {
            Promise<bool> signInPromise = new Promise<bool>();

            AuthenticationService.Instance.SignInAnonymouslyAsync(options as SignInOptions)
                .Future()
                .OnCompleted(validation =>
                {
                    if (!validation.hasException && validation.target.isSucceed)
                    {
                        signInPromise.Resolve(validation.result);
                        return;
                    }
                    
                    signInPromise.Reject(new CofyUnityException(validation.ex));
                });

            return signInPromise.future;
        }

        public bool hasSignInRecord()
        {
            return AuthenticationService.Instance.SessionTokenExists;
        }
    }

    public class CofyUnityException: Exception
    {
        public CofyUnityException(Exception ex)
        {
        }
    }
}