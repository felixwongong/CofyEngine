using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CofyEngine.Editor
{
    [Conditional("UNITY_EDITOR"), AttributeUsage(AttributeTargets.Field)]
    public class CofyDirectoryNameAttribute : PropertyAttribute
    {
    }

    [CustomPropertyDrawer(typeof(CofyDirectoryNameAttribute))]
    public class CofyDirectoryNameAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var rootContainer = new VisualElement();
            var propertyContainer = new VisualElement();
            var field = new TextField() { value = property.stringValue };
            var nameField = new Label(property.displayName);
            var searchButton = new Button();

            #region Styling & Layout

            rootContainer.style.alignItems = Align.Stretch;
            rootContainer.style.flexDirection = FlexDirection.Row;
            rootContainer.style.marginLeft = 5;
            rootContainer.style.marginTop = 3;
            propertyContainer.style.flexDirection = FlexDirection.Row;
            propertyContainer.style.justifyContent = Justify.SpaceBetween;
            propertyContainer.style.flexBasis = Length.Percent(40);
            field.style.flexBasis = Length.Percent(60);
            searchButton.style.backgroundImage =
                new StyleBackground((Texture2D)EditorGUIUtility.IconContent("_Popup").image);
            searchButton.style.width = searchButton.style.height = 20;
            propertyContainer.Add(nameField);
            propertyContainer.Add(searchButton);
            rootContainer.Add(propertyContainer);
            rootContainer.Add(field);

            #endregion

            searchButton.clickable.clicked += () =>
            {
                var path = EditorUtility.OpenFolderPanel("Select Directory", "Assets", "");
                if (string.IsNullOrEmpty(path)) return;

                path = path.AsSpan(path.IndexOf("Assets", StringComparison.Ordinal)).ToString();
                property.stringValue = path;
                property.serializedObject.ApplyModifiedProperties();
                field.value = path;
            };

            return rootContainer;
        }
    }
}