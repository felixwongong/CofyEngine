using System;
using System.Collections.Generic;
using System.Linq;
using CofyEngine.Engine.Core;

namespace CofyEngine
{
    public class StateMachine: IPromiseSM
    {
        private IPromiseState curState;

        private Dictionary<Type, IPromiseState> _stateDictionary;

        public StateMachine()
        {
            _stateDictionary = new Dictionary<Type, IPromiseState>();
        }
        
        public void RegisterState(IPromiseState state)
        {
            _stateDictionary[state.GetType()] = state;
        }

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