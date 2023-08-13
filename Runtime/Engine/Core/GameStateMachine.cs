using UnityEngine;

namespace CofyEngine
{
    public abstract class GameStateMachine: PromiseStateMachine
    {
        private static GameStateMachine _instance;

        public static GameStateMachine instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject($"_GameStateMachine").AddComponent<GameStateMachine>();
                    DontDestroyOnLoad(_instance);
                }
                
                return _instance;
            }
        }

        public abstract void Init();
    }
}