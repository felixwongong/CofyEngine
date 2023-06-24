namespace CofyEngine.Engine
{
    public interface IPromiseSM
    {
        public void GoToNextState<StateType>();
    }
}