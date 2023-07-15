using CofyEngine.Engine.Util;
using Unity.Services.Authentication;

namespace Engine.UGS
{
    public class CofyUGSAuth: Instance<CofyUGSAuth> 
    {
        public Promise<bool> AnonymousSignIn()
        {
            return AuthenticationService.Instance.SignInAnonymouslyAsync()
                .ToPromise();
        }
    }
}