using UnityEngine;

namespace CofyEngine
{
    public class MonoStateMachine : MonoBehaviour, IPromiseSM
    {
        [SerializeField] private bool logging;
        
        private StateMachine _sm;

        protected virtual void Awake()
        {
            _sm = new StateMachine(logging);
        }

        public IPromiseState previousState => _sm.previousState;
        public IPromiseState currentState => _sm.currentState;

        public StateType RegisterState<StateType>(StateType state) where StateType: IPromiseState
        {
            return _sm.RegisterState(state);
        }

        public void GoToState<StateType>(in object param = null)
        {
            _sm.GoToState<StateType>();
        }

        public void GoToStateNoRepeat<StateType>()
        {
            _sm.GoToStateNoRepeat<StateType>();
        }

        public StateType GetState<StateType>() where StateType : IPromiseState
        {
            return _sm.GetState<StateType>();
        }
    }
}