using System;
using System.Collections;
using System.Collections.Generic;
using cofydev.util.UI;
using UnityEngine;

namespace cofydev.util.StateMachine
{
    public abstract class UnityStateMachine : MonoBehaviour, IStateMachine
    {
        private Coroutine currentContext;
        protected IStateContext curStateContext;
        public virtual IStateContext terminateState { get; set; }

        private Dictionary<Type, IStateContext> _stateDictionary;

        private void Awake()
        {
            _stateDictionary = new Dictionary<Type, IStateContext>();
        }

        protected void RegisterState(IStateContext state)
        {
            _stateDictionary[state.GetType()] = state;
        }

        [Obsolete]
        public void GoToNextState(IStateContext context)
        {
            if (currentContext != null) StopCoroutine(currentContext);
            curStateContext = _stateDictionary[context.GetType()];
            if (currentContext == null)
            {
                FLog.LogException(new Exception($"context {context.GetType().ToString()} not registered"));
                return;
            }
            var routine = curStateContext.StartContext(this);
            currentContext = StartCoroutine(routine);
        }

        public abstract void Terminate();
        
        public void GoToNextState<T>() where T: IStateContext
        {
            if (currentContext != null) StopCoroutine(currentContext);
            FLog.Log(typeof(T).ToString());
            curStateContext = _stateDictionary[typeof(T)];
            if (curStateContext == null)
            {
                FLog.LogException(new Exception($"context {typeof(T).ToString()} not registered"));
                return;
            }
            var routine = curStateContext.StartContext(this);

            if (curStateContext == terminateState)
            {
                StartCoroutine(StartTerminateState(routine));
            }
            else
            {
                currentContext = StartCoroutine(routine);
            }
        }

        private IEnumerator StartTerminateState(IEnumerator routine)
        {
            currentContext = StartCoroutine(routine);
            yield return currentContext;
            Terminate();
        }
    }
}