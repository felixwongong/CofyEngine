using System.Collections;
using CofyEngine.Engine.Util.StateMachine;
using CofyEngine.PlayFab;
using Engine.Util;

namespace CofyEngine 
{
    public class BootstrapPlayFab : MonoInstance<BootstrapPlayFab>, IStateContext
    {
#if UNITY_EDITOR
        public string devCustomId = "Tester";
#endif
        
        public IEnumerator StartContext(IStateMachine sm)
        {
            PlayFabController.instance.InitLogin();
            yield break;
        }
    }
}