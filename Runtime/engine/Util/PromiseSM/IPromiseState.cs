namespace CofyEngine
{
    public interface IPromiseState
    {
        protected internal abstract void StartContext(IPromiseSM sm);
        protected internal abstract void OnEndContext();
    }
}