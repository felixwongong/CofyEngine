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
                if (_instance != null) return _instance;
                
                _instance ??= FindObjectOfType<T>();

                if (_instance != null) return _instance;
                
                _instance = new GameObject($"_{typeof(T)}").AddComponent<T>();
                return _instance;
            }
        }
    }
}