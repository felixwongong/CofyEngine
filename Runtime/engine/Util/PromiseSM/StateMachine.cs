using System;
using System.Collections.Generic;
using CofyEngine.Util;

namespace CofyEngine
{
    public class StateMachine<TStateId>: IPromiseSM<TStateId> where TStateId : Enum
    {
        private IPromiseState<TStateId> _prevoutState;
        private IPromiseState<TStateId> _curState;
        public IPromiseState<TStateId> previousState => _prevoutState;
        public IPromiseState<TStateId> currentState => _curState;

        private Dictionary<TStateId, IPromiseState<TStateId>> _stateDictionary = new();

        private Action<IPromiseState<TStateId>, IPromiseState<TStateId>> _logAction;

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
        
        public void SetLogging(Action<IPromiseState<TStateId>, IPromiseState<TStateId>> logAction)
        {
            this._logAction = logAction;
        }

        public void RegisterState(IPromiseState<TStateId> state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (_stateDictionary.ContainsKey(state.id))
            {
                throw new Exception($"State {state.GetType()} already registered");
            }
            _stateDictionary[state.id] = state;
        }

        public void GoToState(TStateId id, in object param = null)
        {
            if (!_curState.isRefNull())
            {
                _curState.OnEndContext();
                _prevoutState = _curState;
            }
            
            if (_curState == null) throw new Exception(string.Format("State {0} not registered", id));
            
            _curState.StartContext(this, param);
            _logAction?.Invoke(_prevoutState, _curState);
        }

        public void GoToStateNoRepeat(TStateId id, in object param = null)
        {
            if(currentState.id.Equals(id))
                GoToState(id, param);
            else if (logging)
                FLog.LogWarning(string.Format("Trying to go to the same state, {0}", id));
        }

        public T GetState<T>(TStateId id) where T : IPromiseState<TStateId>
        {
            if (!_stateDictionary.ContainsKey(id))
            {
                throw new Exception($"State {typeof(T)} not registered");
            }
            return (T) _stateDictionary[id];
        }
    }
}