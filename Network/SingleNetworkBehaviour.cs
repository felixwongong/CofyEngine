using Unity.Netcode;
using UnityEngine;

namespace CM.Util.Singleton
{
    public class SingleNetworkBehaviour<T> : NetworkBehaviour, IPersistentProperty
        where T : Component
    {
        private static T _singleton;

        public static T Singleton
        {
            get
            {
                if (_singleton != null)
                {
                    RegisterAsNetworkObj(_singleton.gameObject);
                    return _singleton;
                }

                ;
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

                RegisterAsNetworkObj(newObj);

                return _singleton;
            }
        }

        protected virtual void Awake()
        {
            foreach (var t in FindObjectsOfType<T>())
                if (t != this)
                    t.gameObject.SetActive(false);
            if (!destroyWithScene) DontDestroyOnLoad(gameObject);
        }

        public virtual bool destroyWithScene => true;

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (gameObject != null)
            {
                NetworkManager.Singleton.RemoveNetworkPrefab(gameObject);
                Destroy(gameObject);
            }
        }

        private static void RegisterAsNetworkObj(GameObject newObj)
        {
            if (_singleton.GetComponent<NetworkObject>() == null && NetworkManager.Singleton.IsListening)
            {
                newObj.AddComponent<NetworkObject>();
                NetworkManager.Singleton.AddNetworkPrefab(newObj);
                if (NetworkManager.Singleton.IsHost)
                    newObj.GetComponent<NetworkObject>().Spawn(((IPersistentProperty)Singleton).destroyWithScene);
            }
        }
    }
}