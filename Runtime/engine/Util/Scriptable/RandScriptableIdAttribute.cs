using System;
using UnityEditor;
using UnityEngine;

namespace CofyEngine.Util.Scriptable
{
    public class RandScriptableIdAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(RandScriptableIdAttribute))]
        public class RandScriptableIdDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                GUI.enabled = false;
                if (string.IsNullOrEmpty(property.stringValue)) property.stringValue = Guid.NewGuid().ToString();

                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }
        }
#endif
    }
}