using UnityEngine;

namespace CofyEngine
{
    public abstract class GameStateMachine : MonoBehaviour
    {
        public virtual void Init()
        {
            FLog.LogWarning("GameStateMachine not implemented yet");
        }
    }
}