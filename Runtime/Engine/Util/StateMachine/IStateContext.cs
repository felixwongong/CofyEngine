using System.Collections;
using UnityEngine;

namespace cofydev.util.StateMachine
{
    public interface IStateContext
    {
        public IEnumerator StartContext(IStateMachine sm);
    }
}