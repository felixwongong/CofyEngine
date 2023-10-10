using System.Collections;
using CofyEngine.Util.StateMachine;

namespace CofyEngine.Core
{
    public class LoginState: IStateContext
    {
        public IEnumerator StartContext(IStateMachine sm)
        {
            yield break;
        }
    }
}