﻿using System;
using UnityEngine;

namespace cofydev.util.StateMachine
{
    public abstract class UnityStateMachine : MonoBehaviour, IStateMachine
    {
        private Coroutine currentContext;
        protected IStateContext curStateContext;

        public void GoToNextState(IStateContext context)
        {
            if (currentContext != null) StopCoroutine(currentContext);
            curStateContext = context;
            var routine = curStateContext.StartContext(this);
            currentContext = StartCoroutine(routine);
        }

        public void GoToNextStateClientRpc(string stateName)
        {
            var state = (EState)stateName;
            var context = FindObjectOfType((Type)state) as IStateContext;
            GoToNextState(context);
        }
    }
}