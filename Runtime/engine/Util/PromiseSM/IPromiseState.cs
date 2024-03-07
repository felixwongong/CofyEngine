namespace CofyEngine
{
    public interface IState<TStateId>
    {
        TStateId id { get; }
        void StartContext(IStateMachine<TStateId> sm, object param);
        void OnEndContext();
   }
}