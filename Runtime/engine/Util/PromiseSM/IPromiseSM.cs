namespace CofyEngine
{
    public interface IPromiseSM
    {
        public IPromiseState currentState { get; }
        
        public void RegisterState(IPromiseState state);
        public void GoToState<StateType>();
        public void GoToStateNoRepeat<StateType>();
        public StateType GetState<StateType>() where StateType : IPromiseState;
    }
}