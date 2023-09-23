using System;

namespace CofyEngine.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var sceneObject = GetSceneObject(property.stringValue);
                var scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
                if (scene == null)
                {
                    property.stringValue = "";
                }
                else if (scene.name != property.stringValue)
                {
                  
                    var sceneObj = GetSceneObject(scene.name);
                    if (sceneObj == null)
                    {
                        FLog.LogWarning("The scene " + scene.name +
                                         " cannot be used. To use this scene add it to the build settings for the project, or add to addressable folder");
                        return;
                    }
                    
                    property.stringValue = scene.name;
                }
            }
            else
                EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
        }

        protected SceneAsset GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
            {
                return null;
            }

            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.path.IndexOf(sceneObjectName, StringComparison.Ordinal) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }
            
            var path = string.Format("{0}/{1}.unity", AssetPath.SCENE_ROOT, sceneObjectName);
            var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            return asset;
        }
    }
    
    public class SceneAttribute : PropertyAttribute
    {
    }
}
