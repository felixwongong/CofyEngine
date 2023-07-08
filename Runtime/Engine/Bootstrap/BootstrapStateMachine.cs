using System;
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
        try
        {
            var scene = ClientMain.instance.firstScene;
            LevelManager.instance.LoadLevelFull(scene,
                after: (old, newScene) => { GameStateMachine.instance.Init(); });
        }
        catch (Exception e)
        {
            FLog.LogException(e);
        }
    }
}