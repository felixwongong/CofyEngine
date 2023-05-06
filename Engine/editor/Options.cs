using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Attribute = System.Attribute;
using Type = System.Type;

namespace cofydev.util.Editor
{
    //TODO complete in the future, for rendering enum like class as drop down
    [AttributeUsage(AttributeTargets.Field)]
    public class Options : Attribute
    {
        public Type optionType;
        public Options(Type type)
        {
            optionType = type;
        }
    }

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class OptionsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            FLog.Log("orewrweruowier");
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo[] fields = target.GetType().GetFields(flags);
            
            for (var i = 0; i < fields.Length; i++)
            {
                var optionAttr = Attribute.GetCustomAttribute(fields[i], typeof(Options)) as Options;
                if(optionAttr == null) return;

                FieldInfo[] options = optionAttr.optionType.GetFields(flags);
                FLog.Log(options.ToString());
            }
        }
    }
}