using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

namespace cofydev.util.UI
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
    public class SerializableDictionaryPropertyDrawer : PropertyDrawer
    {
        private const float lineHeight = 20f;
        private const float padding = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int count = property.FindPropertyRelative("keys").arraySize;
            return lineHeight * (count + 2) + padding * (count + 1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty keys = property.FindPropertyRelative("keys");
            SerializedProperty values = property.FindPropertyRelative("values");
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, lineHeight), property.displayName);
            position.y += lineHeight + padding;

            EditorGUI.indentLevel++;
            for (int i = 0; i < keys.arraySize; i++)
            {
                Rect deleteButtonRect = new Rect(position.x, position.y, position.width * 0.1f - padding, lineHeight);
                Rect keyRect = new Rect(position.x + position.width * 0.1f, position.y, position.width * 0.45f, lineHeight);
                Rect valueRect = new Rect(position.x + position.width * 0.55f, position.y, position.width * 0.45f, lineHeight);

                EditorGUI.PropertyField(keyRect, keys.GetArrayElementAtIndex(i), GUIContent.none);
                EditorGUI.PropertyField(valueRect, values.GetArrayElementAtIndex(i), GUIContent.none);

                position.y += lineHeight + padding;


                if (GUI.Button(deleteButtonRect, "Del"))
                {
                    keys.DeleteArrayElementAtIndex(i);
                    values.DeleteArrayElementAtIndex(i);
                    property.serializedObject.ApplyModifiedProperties();
                    EditorGUIUtility.ExitGUI();
                }
            }

            if (GUI.Button(new Rect(position.x, position.y, position.width, lineHeight), "Add entry"))
            {
                keys.arraySize++;
                values.arraySize++;
            }


            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }
    }
#endif

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] internal List<TKey> keys = new List<TKey>();
        [SerializeField] internal List<TValue> values = new List<TValue>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in this)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            for (int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }

        public bool TrySelectEntry(TKey key, out TValue value)
        {
            if (TryGetValue(key, out value))
            {
                return true;
            }

            value = default(TValue);
            return false;
        }

        public bool DeleteEntry(TKey key)
        {
            return Remove(key);
        }

    }
    public static class SerializableDictionaryExtension {
        public static SerializableDictionary<A, B> ToSerializable<A, B>(this Dictionary<A, B> dict)
        {
            var serializedDictionary = new SerializableDictionary<A, B>
            {
                keys = dict.Keys.ToList(),
                values = dict.Values.ToList()
            };

            serializedDictionary.AddRange(dict);
            return serializedDictionary;
        }
    }
}