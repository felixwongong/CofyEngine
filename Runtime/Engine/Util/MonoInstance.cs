using UnityEngine;

namespace Engine.Util
{
    public class MonoInstance<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                GameObject go = new GameObject($"_{typeof(T)}");
                _instance = go.AddComponent<T>();
                return _instance;
            }
        }
    }
}