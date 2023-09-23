using System.Collections.Generic;
using UnityEngine;

namespace CofyEngine.Engine.Util.UI
{
    public class StateControl : UIBinder<StateControl>
    {
        public List<GameObject> controlTargets;

        public State<string> _currentState;

        protected override void Awake()
        {
            base.Awake();

            if (controlTargets.Count <= 0)
            {
                FLog.LogError("there are no control targets");
                return;
            }

            _currentState = new State<string>(controlTargets[0].name, RedrawAction);
            RedrawAction(_currentState.Value);
        }

        public void RedrawAction(string nextState)
        {
            for (var i = 0; i < controlTargets.Count; i++)
            {
                controlTargets[i].SetActive(controlTargets[i].name == _currentState);
            }
        }
    }
}