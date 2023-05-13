using System;
using cofydev.util.StateMachine;
using Unity.Netcode;
using UnityEngine;

namespace CofyEngine.Network
{
    public abstract class NetworkStateMachine : NetworkBehaviour, IStateMachine
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

        public abstract void Terminate();

        [ClientRpc]
        public void GoToNextStateClientRpc(string stateName)
        {
            var state = (EState)stateName;
            var context = FindObjectOfType((Type)state) as IStateContext;
            GoToNextState(context);
        }
    }
}