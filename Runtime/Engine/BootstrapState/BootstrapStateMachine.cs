using CofyEngine;
using UnityEngine;

public class BootstrapStateMachine : MonoBehaviour
{
    [SerializeField] private GameStateMachine _gsm;
    
    private StateMachine sm;

    private void Awake()
    {
        sm = new StateMachine();
        
        sm.RegisterState(GetComponent<BootstrapUI>());
        sm.RegisterState(GetComponent<BootstrapUGS>());
        sm.RegisterState(GetComponent<TerminateState>());

        sm.GoToNextState<BootstrapUI>();
    }
}
