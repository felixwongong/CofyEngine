using System;
using System.Collections.Generic;
using System.Linq;
using CofyEngine.Engine.Core;
using CofyEngine.Engine.Util;

namespace CofyEngine
{
    public class StateMachine: IPromiseSM
    {
        private IPromiseState prevState;
        private IPromiseState curState;

        private Dictionary<Type, IPromiseState> _stateDictionary = new();

        private Action<IPromiseState, IPromiseState> _logAction;

        public StateMachine(bool logging = false)
        {
            if (logging)
            {
                _logAction = (oldState, newState) =>
                {
                    FLog.Log(oldState != null
                        ? string.Format("Transit from state [{0}] to [{1}]", oldState.GetTName(), newState.GetTName())
                        : string.Format("Start initial state [{0}]", newState.GetTName()));
                };
            }
        }
        
        public void SetLogging(Action<IPromiseState, IPromiseState> logAction)
        {
            this._logAction = logAction;
        }
        
        public void RegisterState(IPromiseState state)
        {
            _stateDictionary[state.GetType()] = state;
        }

        public void GoToNextState<T>()
        {
            prevState = curState;
            if (!_stateDictionary.TryGetValue(typeof(T), out curState))
            {
                curState = _stateDictionary.Values.First(state => state is T);
            }
            MainThreadExecutor.instance.QueueAction(() =>
            {
                curState.StartContext(this);
                _logAction?.Invoke(prevState, curState);
            });
        }
    }
}