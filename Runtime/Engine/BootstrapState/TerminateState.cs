using UnityEngine;

namespace CofyEngine
{
    public class TerminateState : MonoBehaviour, IPromiseState
    {
        [SerializeField] private GameStateMachine _gsm;
        void IPromiseState.StartContext(IPromiseSM sm)
        {
            _gsm.Init();
        }
    }
}