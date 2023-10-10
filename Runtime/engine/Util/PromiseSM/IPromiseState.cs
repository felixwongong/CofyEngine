namespace CofyEngine
{
    public interface IPromiseState
    {
        void StartContext(IPromiseSM sm);
        void OnEndContext();
   }
}