using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using WebSocketSharp;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;


public static class FLog
    {
        private static Dictionary<string, string> _propertyMap = new ();
        private static StringBuilder _sb = new ();
        
        public static void Log(string inMsg, object inObj = null)
        {
            Debug.Log(MakeLogString(inMsg, inObj));
        }

        public static void LogWarning(string inMsg, object inObj = null)
        {
            Debug.LogWarning(MakeLogString(inMsg, inObj));
        }

        public static void LogError(string inMsg, object inObj = null)
        {
            Debug.LogError(MakeLogString(inMsg, inObj));
        }

        public static void Log<T>(T inObj)
        {
            Debug.Log(MakeLogString("", inObj));
        }

        private static string MakeLogString(string inMsg = "", object inObj = null)
        {

            Type type = new StackTrace().GetFrame(2).GetMethod().DeclaringType;

            _sb.Clear();
            
            while (type?.DeclaringType != null)
            {
                type = type?.DeclaringType;
            }
            
            _sb.AppendFormat("[{0}]: ", type?.Name);
            if (!inMsg.IsNullOrEmpty()) _sb.AppendFormat("{0}\n", inMsg);
            MakeFieldString(inObj, _sb);

            return _sb.ToString();
        }

        private static void FormatString(string title, Dictionary<string, string> keyValue, in StringBuilder sb)
        {
            int maxKeyLength = keyValue.Keys.Select(k => k.Length).Max();

            sb.AppendFormat("\n{0}\n{1}", title, new string('-', title.Length));
            
            foreach (var (k, v) in keyValue)
            {
                sb.AppendFormat("\n{0}: {1}", k.PadRight(maxKeyLength), v);
            }
        }

        private static void MakeFieldString(object obj, in StringBuilder sb)
        {
            _propertyMap.Clear();
            
            //Handling struct/class object
            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var fieldInfo in fields)
            {
                var val = fieldInfo.GetValue(obj);
                if(val == null) continue;
                _propertyMap[fieldInfo.Name] = fieldInfo.GetValue(obj).ToString();
            }

            FormatString(obj.GetType().ToString(), _propertyMap, sb);
        }

        public static void LogException(Exception e)
        {
            Debug.LogException(new Exception(MakeLogString(e.Message), e));
        }
    }