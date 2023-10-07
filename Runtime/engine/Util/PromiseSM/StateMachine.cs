using System;
using System.Collections.Generic;
using System.Linq;
using CofyEngine.Engine.Core;
using CofyEngine.Engine.Util;

namespace CofyEngine
{
    public class StateMachine: IPromiseSM
    {
        private IPromiseState _prevState;
        private IPromiseState _curState;
        public IPromiseState currentState => _curState;

        private Dictionary<Type, IPromiseState> _stateDictionary = new();

        private Action<IPromiseState, IPromiseState> _logAction;

        private bool logging;

        public StateMachine(bool logging = false)
        {
            this.logging = logging;
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
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (_stateDictionary.ContainsKey(state.GetType()))
            {
                throw new Exception($"State {state.GetType()} already registered");
            }
            _stateDictionary[state.GetType()] = state;
        }

        public void GoToState<T>()
        {
            if(!_prevState.isRefNull()) 
                _prevState.OnEndContext();
                
            _prevState = _curState;
            if (!_stateDictionary.TryGetValue(typeof(T), out _curState))
            {
                _curState = _stateDictionary.Values.First(state => state is T);
            }
            MainThreadExecutor.instance.QueueAction(() =>
            {
                _curState.StartContext(this);
                _logAction?.Invoke(_prevState, _curState);
            });
        }

        public void GoToStateNoRepeat<StateType>()
        {
            if(currentState is not StateType)
                GoToState<StateType>();
            else if (logging)
                FLog.LogWarning(string.Format("Trying to go to the same state, {0}", typeof(StateType)));
        }

        public T GetState<T>() where T : IPromiseState
        {
            if (!_stateDictionary.ContainsKey(typeof(T)))
            {
                throw new Exception($"State {typeof(T)} not registered");
            }
            return (T) _stateDictionary[typeof(T)];
        }
    }
}