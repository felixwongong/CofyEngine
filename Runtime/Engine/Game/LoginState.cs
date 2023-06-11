using System.Collections;
using CofyEngine.Runtime.Engine.Util.StateMachine;
using UnityEngine;

namespace CofyEngine.Engine.Game
{
    public class LoginState: IStateContext
    {
        public IEnumerator StartContext(IStateMachine sm)
        {
            yield break;
        }
    }
}