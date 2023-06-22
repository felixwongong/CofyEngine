namespace CofyEngine.Engine.Util
{
    public class Instance<T> where T: new ()
    {
        internal T _instance;
        public T instance => _instance ??= new T();
    }
}