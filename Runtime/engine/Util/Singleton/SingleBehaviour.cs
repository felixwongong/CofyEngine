using UnityEngine;

namespace CM.Util.Singleton
{
    public class SingleBehaviour<T> : MonoBehaviour, IPersistentProperty
        where T : Component
    {
        private static T _singleton;

        public static T Singleton
        {
            get
            {
                if (_singleton != null) return _singleton;
                var objs = (T[])FindObjectsOfType(typeof(T));
                if (objs.Length > 0)
                    _singleton = objs[0];
                if (objs.Length > 1)
                    Debug.LogError("There are more than 1 instances of " + typeof(T));
                if (_singleton) return _singleton;
                var newObj = new GameObject
                {
                    name = $"_{typeof(T).Name}"
                };
                _singleton = newObj.AddComponent<T>();

                return _singleton;
            }
            set => _singleton = value;
        }

        protected virtual void Awake()
        {
            if (!destroyWithScene) DontDestroyOnLoad(Singleton.gameObject);
        }

        public virtual bool destroyWithScene => true;
    }
}