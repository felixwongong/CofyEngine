using System;
using UnityEditor;
using UnityEngine;

namespace CofyEngine.Editor
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CofyDirectoryPath: Attribute
    {
    }
    
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class CofyDirectoryPathEditor : UnityEditor.Editor
    {
        private string _path;
        public override void OnInspectorGUI()
        {
            _OnInspectorGUI();
        }

        private void _OnInspectorGUI()
        {
            var fields = target.GetType().GetFields();

            for (var i = 0; i < fields.Length; i++)
            {
                if (Attribute.GetCustomAttribute(fields[i], typeof(CofyDirectoryPath)) is not CofyDirectoryPath path) continue;

                
                GUILayout.BeginHorizontal();
                GUILayout.Label(fields[i].Name);
                if (GUILayout.Button(EditorGUIUtility.IconContent("_Popup"), EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                {
                    _path = EditorUtility.OpenFolderPanel("Select Directory", "Assets", "");
                    if (string.IsNullOrEmpty(_path)) continue;

                    _path = _path.AsSpan(_path.IndexOf("Assets", StringComparison.Ordinal)).ToString();
                    fields[i].SetValue(target, _path);
                }
                GUILayout.TextField(_path);
                GUILayout.EndHorizontal();
            }
        }
    }
}