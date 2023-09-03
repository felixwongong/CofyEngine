using CofyEngine;
using CofyEngine.Engine;
using CofyEngine.Engine.Core;

public class BootstrapStateMachine : MonoStateMachine
{
    protected override void RegisterAllState()
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
        GameStateMachine.instance.Init();
    }
}