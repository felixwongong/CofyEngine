using System.Collections;
using CofyEngine.Engine.Util.StateMachine;
using UnityEngine;

namespace CofyEngine.Engine.Game
{
    public class GameStateMachine: PromiseStateMachine
    {
        private static GameStateMachine _instance;

        public static GameStateMachine instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject($"_GameStateMachine").AddComponent<GameStateMachine>();
                }
                
                return _instance;
            }
        }

        public void Init()
        {

        }
    }
}