using cofydev.util;
using cofydev.util.StateMachine;
using CofyEngine.Engine.Game;
using CofyEngine.Engine.util.Editor;
using CofyUI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapStateMachine : UnityStateMachine
{
    [Scene] 
    [SerializeField]
    private string firstScene;

    private void Start()
    {
        var bootstrapUI = GetComponent<BootstrapUI>();
        RegisterState(bootstrapUI);

        terminateState = bootstrapUI;

        GoToNextState(bootstrapUI.GetType());
    }

    public override void Terminate()
    {
        Promise<bool> promise = SceneManager.LoadSceneAsync(firstScene).ToPromise();
        promise.Succeed += _ =>
        {
            GameStateMachine.instance.Init();
            LoadingScreen.instance.SetGoActive(false);
        };
    }
}
