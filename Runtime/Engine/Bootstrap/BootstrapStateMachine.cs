using cofydev.util.StateMachine;
using CofyEngine;
using CofyEngine.Engine.Game;
using CofyEngine.Engine.util.Editor;
using UnityEngine;

public class BootstrapStateMachine : UnityStateMachine
{
    [Scene] 
    [SerializeField]
    private string firstScene;

    private void Start()
    {
        var bootstrapUI = GetComponent<BootstrapUI>();
        var bootstrapUGS = GetComponent<BootstrapUGS>();
        
        RegisterState(bootstrapUI);
        RegisterState(bootstrapUGS);

        terminateState = bootstrapUGS;

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
