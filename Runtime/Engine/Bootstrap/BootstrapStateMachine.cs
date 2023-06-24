using CofyEngine;
using CofyEngine.Engine;
using CofyEngine.Engine.Game;

public class BootstrapStateMachine : PromiseStateMachine
{
    private void Start()
    {
        RegisterState(GetComponent<BootstrapUI>());
        RegisterState(GetComponent<BootstrapPlayFab>());
        RegisterState(new TerminateState());

        GoToNextState<BootstrapUI>();
    }

    private class TerminateState : IPromiseState
    {
        void IPromiseState.StartContext(IPromiseSM sm)
        {
            LevelManager.Singleton.LoadLevelFull(ClientMain.instance.firstScene,
                after: (old, newScene) => { GameStateMachine.instance.Init(); });
        }
    }
}