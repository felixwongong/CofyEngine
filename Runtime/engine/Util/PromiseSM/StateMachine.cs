using System;
using System.Collections.Generic;
using CofyEngine.Util;

namespace CofyEngine
{
    public struct StateChangeRecord<TStateId>
    {
        public IState<TStateId> oldState;
        public IState<TStateId> newState;
    }
    
    public class StateMachine<TStateId>: IStateMachine<TStateId> where TStateId : Enum
    {
        private IState<TStateId> _prevoutState;
        private IState<TStateId> _curState;
        public IState<TStateId> previousState => _prevoutState;
        public IState<TStateId> currentState => _curState;

        private Dictionary<TStateId, IState<TStateId>> _stateDictionary = new();

        
        public SmartEvent<StateChangeRecord<TStateId>> onBeforeStateChange = new();

        private bool logging;
        
        IRegistration loggingReg;
        
        public StateMachine(bool logging = false)
        {
            this.logging = logging;
            if (logging)
            {
                loggingReg = onBeforeStateChange.Register(

                    rec =>
                    {
                        FLog.Log(rec.oldState != null
                            ? string.Format("Transit from state [{0}] to [{1}]", rec.oldState.GetTName(),
                                rec.newState.GetTName())
                            : string.Format("Start initial state [{0}]", rec.newState.GetTName()));
                    }
                );
            }
        }
        
        public void RegisterState(IState<TStateId> state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (_stateDictionary.ContainsKey(state.id))
            {
                throw new Exception($"State {state.GetType()} already registered");
            }
            FLog.Log($"Register state {state.id}");
            _stateDictionary[state.id] = state;
        }

        public void GoToState(TStateId id, in object param = null)
        {
            if (!_curState.isRefNull())
            {
                _curState.OnEndContext();
                _prevoutState = _curState;
            }
            
            if (!_stateDictionary.TryGetValue(id, out _curState))
                throw new Exception(string.Format("State {0} not registered", id));
            
            onBeforeStateChange?.Invoke(new StateChangeRecord<TStateId>() {oldState = _prevoutState, newState = _curState});
            _curState.StartContext(this, param);
        }

        public void GoToStateNoRepeat(TStateId id, in object param = null)
        {
            if(currentState.id.Equals(id))
                GoToState(id, param);
            else if (logging)
                FLog.LogWarning(string.Format("Trying to go to the same state, {0}", id));
        }

        public T GetState<T>(TStateId id) where T : IState<TStateId>
        {
            if (!_stateDictionary.ContainsKey(id))
            {
                throw new Exception($"State {typeof(T)} not registered");
            }
            return (T) _stateDictionary[id];
        }
    }
    
    public class SingletonStateMachine<T, TStateId>: Instance<T> where T : new() where TStateId : Enum
    {
        protected StateMachine<TStateId> sm;

        protected SingletonStateMachine()
        {
            sm = new StateMachine<TStateId>();
        }
    }
}