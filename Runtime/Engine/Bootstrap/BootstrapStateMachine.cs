using CofyEngine;
using CofyEngine.Engine;
using CofyEngine.Engine.Game;

public class BootstrapStateMachine : PromiseStateMachine
{
    private void Start()
    {
        RegisterState(GetComponent<BootstrapUI>());
        RegisterState(GetComponent<BootstrapUGS>());
        RegisterState(new TerminateState());

        GoToNextState<BootstrapUI>();
    }
}

class TerminateState : IPromiseState
{
    void IPromiseState.StartContext(IPromiseSM sm)
    {
        LevelManager.Singleton.LoadLevelFull(ClientMain.instance.firstScene,
            after: (old, newScene) => { GameStateMachine.instance.Init(); });
    }
}