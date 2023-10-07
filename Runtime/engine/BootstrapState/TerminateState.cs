namespace CofyEngine
{
    public class TerminateState : IPromiseState
    {
        private GameStateMachine gsm;
        
        public TerminateState(GameStateMachine gsm)
        {
            this.gsm = gsm;
        }
        
        void IPromiseState.StartContext(IPromiseSM sm)
        {
            gsm.Init();
        }

        void IPromiseState.OnEndContext() { }
    }
}