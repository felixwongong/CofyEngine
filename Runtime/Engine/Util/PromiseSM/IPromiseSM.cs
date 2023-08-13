namespace CofyEngine
{
    public interface IPromiseSM
    {
        public void GoToNextState<StateType>();
    }
}