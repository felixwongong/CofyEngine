using System.Collections;
using UnityEngine;

namespace CofyEngine.Runtime.Engine.Util.StateMachine
{
    public interface IStateContext
    {
        public IEnumerator StartContext(IStateMachine sm);
    }
}