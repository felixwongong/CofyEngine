using UnityEditor;
using UnityEngine;

namespace cofydev.util.UI.Editor
{
    [CustomEditor(typeof(StateControl), true)]
    public class StateControlEditor : UnityEditor.Editor
    {
        private GUIStyle _headerStyle;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_headerStyle == null)
            {
                _headerStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            }

            StateControl sc = target as StateControl;
            if (sc == null)
            {
                Debug.Log($"target {target} is not StateControl object!");
                return;
            }

            var controlTargets = sc.controlTargets;
            if(controlTargets == null) return;

            for (var i = 0; i < controlTargets.Count; i++)
            {
                var target = controlTargets[i];
                if (GUILayout.Button(target.name))
                {
                    sc._currentState.Value = target.name;
                    sc.RedrawAction(target.name);
                }
            }
        }
    }
}