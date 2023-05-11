using UnityEngine;

namespace CofyUI
{
    public class UIInstance<T>: MonoBehaviour
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                _instance ??= UIRoot.Singleton.GetInstance<T>();
                return _instance;
            }
        }
    }
}