using CofyEngine.Engine.Util.StateMachine;
using CofyEngine;
using CofyEngine.Engine.Game;
using CofyEngine.Engine.Util.Editor;
using UnityEngine;

public class BootstrapStateMachine : UnityStateMachine
{
    [Scene] 
    [SerializeField]
    private string firstScene;

    private void Start()
    {
        var bootstrapUI = GetComponent<BootstrapUI>();
        var bootstrapPlayFab = GetComponent<BootstrapPlayFab>();
        
        RegisterState(bootstrapUI);
        RegisterState(bootstrapPlayFab);

        terminateState = bootstrapPlayFab;

        GoToNextState(bootstrapUI.GetType());
    }

    public override void Terminate()
    {
        LevelManager.Singleton.LoadLevelFull(firstScene, after: (old, newScene) =>
        {
            GameStateMachine.instance.Init();
        });
    }
}
