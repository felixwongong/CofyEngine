using PlayFab;
using PlayFab.ClientModels;

namespace CofyEngine.PlayFab
{
    public class PlayFabAuth
    {
        public static Promise<LoginResult> LoginWithCustomID(PlayFabClientInstanceAPI client, string customID, bool createAccount = true)
        {
            Promise<LoginResult> loginPromise = new Promise<LoginResult>();

            var request = new LoginWithCustomIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = createAccount,
                CustomId = customID,
            };
            
            client.LoginWithCustomID(request, loginPromise.Resolve, (err) =>
            {
                loginPromise.Reject(err.GenerateErrorReport());
            });

            return loginPromise;
        }
    }
}