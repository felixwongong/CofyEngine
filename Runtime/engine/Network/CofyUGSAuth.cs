using CofyEngine.Util;
using Unity.Services.Authentication;

namespace Engine.UGS
{
    public class CofyUGSAuth: Instance<CofyUGSAuth> 
    {
        public Future<bool> AnonymousSignIn()
        {
            return AuthenticationService.Instance.SignInAnonymouslyAsync()
                .Future();
        }
    }
}