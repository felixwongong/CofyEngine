namespace cofydev.util.StateMachine
{
    public interface IStateMachine
    {
        public void GoToNextState(IStateContext context);
        public void GoToNextStateClientRpc(string stateName);
    }
}