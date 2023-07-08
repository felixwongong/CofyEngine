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

        private Queue<Action> actionQueue = new Queue<Action>();

        private void Awake()
        {
            _stateDictionary = new Dictionary<Type, IPromiseState>();
        }

        private void Update()
        {
            while (actionQueue.Count > 0)
            {
                var action = actionQueue.Dequeue();
                action();
            }
        }

        protected void RegisterState(IPromiseState state)
        {
            _stateDictionary[state.GetType()] = state;
        }

        public void GoToNextState<StateType>()
        {
            if (!_stateDictionary.TryGetValue(typeof(StateType), out curState))
            {
                curState = _stateDictionary.Values.First(state => state is StateType);
            }
            actionQueue.Enqueue(() => curState.StartContext(this));
        }
    }
}