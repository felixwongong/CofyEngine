using System.Collections;
using CofyEngine.Engine.Util.StateMachine;

namespace CofyEngine.Engine.Core
{
    public class LoginState: IStateContext
    {
        public IEnumerator StartContext(IStateMachine sm)
        {
            yield break;
        }
    }
}