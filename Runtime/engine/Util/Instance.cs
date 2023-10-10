namespace CofyEngine.Util
{
    public class Instance<T> where T: new ()
    {
        private static T _instance;
        public static T instance => _instance ??= new T();
    }
}