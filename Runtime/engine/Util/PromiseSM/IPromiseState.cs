namespace CofyEngine
{
    public interface IPromiseState
    {
        void StartContext(IPromiseSM sm, object param);
        void OnEndContext();
   }
}