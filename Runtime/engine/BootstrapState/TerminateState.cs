namespace CofyEngine
{
    public class TerminateState : IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.Terminate;
        
        private GameStateMachine gsm;
        
        public TerminateState(GameStateMachine gsm)
        {
            this.gsm = gsm;
        }

        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            gsm.Init();
        }

        public void OnEndContext() { }
    }
}