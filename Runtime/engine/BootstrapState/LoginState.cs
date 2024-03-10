namespace CofyEngine
{
    public class LoginState: BaseState<BootStateId>
    {
        public override BootStateId id => BootStateId.Login;

        protected internal override void StartContext(IStateMachine<BootStateId> sm, object param)
        {
            var auth = CofyUnityAuth.instance;
            
            auth.Init()
                .OnSucceed(_ =>
                {
                    if (auth.hasSignInRecord())
                    {
                        auth.SignInAnonymous()
                            .OnSucceed(_ => sm.GoToState(BootStateId.GamePreload));
                    }
                });
        }
    }
}