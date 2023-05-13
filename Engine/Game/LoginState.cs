using System.Collections;
using cofydev.util;
using cofydev.util.StateMachine;
using UnityEngine.SceneManagement;

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