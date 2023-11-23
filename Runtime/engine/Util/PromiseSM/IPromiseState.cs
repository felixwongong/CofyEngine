namespace CofyEngine
{
    public interface IPromiseState<TStateId>
    {
        TStateId id { get; }
        void StartContext(IPromiseSM<TStateId> sm, object param);
        void OnEndContext();
   }
}