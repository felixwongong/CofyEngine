namespace CofyEngine
{
    public interface IPromiseSM<TStateId>
    {
        public void RegisterState(IPromiseState<TStateId> state);
        public void GoToState(TStateId id, in object param = null);
        public void GoToStateNoRepeat(TStateId id, in object param = null);
        public T GetState<T>(TStateId id) where T : IPromiseState<TStateId>;
    }
}