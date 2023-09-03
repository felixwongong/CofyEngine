using System;
using System.Collections.Generic;
using System.Linq;
using CofyEngine.Engine.Core;
using UnityEngine;

namespace CofyEngine
{
    public abstract class MonoStateMachine : MonoBehaviour, IPromiseSM
    {
        private IPromiseState curState;

        private Dictionary<Type, IPromiseState> _stateDictionary;

        protected virtual void Awake()
        {
            _stateDictionary = new Dictionary<Type, IPromiseState>();
            RegisterAllState();
        }

        protected void RegisterState(IPromiseState state)
        {
            _stateDictionary[state.GetType()] = state;
        }

        protected abstract void RegisterAllState();

        public void GoToNextState<StateType>()
        {
            if (!_stateDictionary.TryGetValue(typeof(StateType), out curState))
            {
                curState = _stateDictionary.Values.First(state => state is StateType);
            }
            MainThreadExecutor.instance.QueueAction(() => curState.StartContext(this));
        }
    }
}