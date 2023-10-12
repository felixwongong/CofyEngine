namespace CofyEngine
{
    public interface IPromiseSM
    {
        public IPromiseState currentState { get; }
        
        public StateType RegisterState<StateType>(StateType state) where StateType: IPromiseState;
        public void GoToState<StateType>();
        public void GoToStateNoRepeat<StateType>();
        public StateType GetState<StateType>() where StateType : IPromiseState;
    }
}