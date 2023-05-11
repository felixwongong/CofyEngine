using System;
using System.Collections.Generic;
using cofydev.util.UI;
using UnityEngine;

namespace cofydev.util.StateMachine
{
    public abstract class UnityStateMachine : MonoBehaviour, IStateMachine
    {
        private Coroutine currentContext;
        protected IStateContext curStateContext;

        private Dictionary<string, IStateContext> _stateDictionary;

        private void Awake()
        {
            _stateDictionary = new Dictionary<string, IStateContext>();
        }

        protected void RegisterState(IStateContext state)
        {
            _stateDictionary[state.ToString()] = state;
        }

        public void GoToNextState(IStateContext context)
        {
            if (currentContext != null) StopCoroutine(currentContext);
            curStateContext = _stateDictionary[context.GetType().ToString()];
            if (currentContext == null)
            {
                FLog.LogException(new Exception($"context {context.GetType().ToString()} not registered"));
                return;
            }
            var routine = curStateContext.StartContext(this);
            currentContext = StartCoroutine(routine);
        }
        
        public void GoToNextState<T>() where T: IStateContext
        {
            if (currentContext != null) StopCoroutine(currentContext);
            FLog.Log(typeof(T).ToString());
            curStateContext = _stateDictionary[typeof(T).ToString()];
            if (curStateContext == null)
            {
                FLog.LogException(new Exception($"context {typeof(T).ToString()} not registered"));
                return;
            }
            var routine = curStateContext.StartContext(this);
            currentContext = StartCoroutine(routine);
        }
    }
}