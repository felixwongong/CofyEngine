using System;
using System.Collections.Generic;
using System.Linq;
using CofyEngine.Engine.Util.StateMachine;
using UnityEngine;

namespace CofyEngine.Engine
{
    public abstract class PromiseStateMachine : MonoBehaviour, IPromiseSM
    {
        private IPromiseState curState;

        private Dictionary<Type, IPromiseState> _stateDictionary;

        private void Awake()
        {
            _stateDictionary = new Dictionary<Type, IPromiseState>();
        }

        protected void RegisterState(IPromiseState state)
        {
            _stateDictionary[state.GetType()] = state;
        }

        public void GoToNextState<StateType>()
        {
            IPromiseState curState;
            if (!_stateDictionary.TryGetValue(typeof(StateType), out curState))
            {
                curState = _stateDictionary.Values.First(state => state is StateType);
            }
            curState.StartContext(this);
        }
    }
}