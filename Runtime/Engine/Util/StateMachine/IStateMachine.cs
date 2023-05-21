namespace cofydev.util.StateMachine
{
    public interface IStateMachine
    {
        public void GoToNextState(IStateContext context);

        public void GoToNextState<T>() where T: IStateContext;

        public void Terminate();
    }
}