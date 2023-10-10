using System.Collections;
using UnityEngine;

namespace CofyEngine.Util.StateMachine
{
    public interface IStateContext
    {
        public IEnumerator StartContext(IStateMachine sm);
    }
}