using System;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CofyEngine.Editor
{
    [CustomPropertyDrawer(typeof(CofySceneAttribute))]
    public class SceneDrawer : PropertyDrawer
    {
        private SceneAsset sceneAsset;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                if(!property.stringValue.isNullOrEmpty())
                {
                    var path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(property.stringValue).First());
                    sceneAsset = (SceneAsset)AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset));
                }
                
                sceneAsset = (SceneAsset)EditorGUI.ObjectField(position, label, sceneAsset, typeof(SceneAsset), true);
                property.stringValue = sceneAsset == null ? string.Empty : sceneAsset.name;
            }
            else
                EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
        }
    }
    
    [Conditional("UNITY_EDITOR"), AttributeUsage(AttributeTargets.Field)]
    public class CofySceneAttribute : PropertyAttribute
    {
    }
}
