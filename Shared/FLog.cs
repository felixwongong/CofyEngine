using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;


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

        public static void LogObject(object inObj)
        {
            Debug.Log(MakeLogString("", inObj));
        }
        
        private static string MakeLogString(string inMsg = "", object inObj = null)
        {
            
            Type type = new StackTrace().GetFrame(2).GetMethod().DeclaringType;
            
            try
            {
                var message = string.IsNullOrEmpty(inMsg) ? "" : string.Format("{0}\n", inMsg);
                var obj = inObj == null ? "" : MakeFieldString(inObj);
                while (type?.DeclaringType != null)
                {
                    type = type?.DeclaringType;
                }

                return string.Format("[{0}]: {1}{2}", type?.Name, message, obj);

            }
            catch (Exception e)
            {
                return string.Format("[{0}]: {1}", type?.Name, e);
            }
        }

        private static string FormatString(string title, Dictionary<string, string> keyValue)
        {
            int maxKeyLength = keyValue.Keys.Select(k => k.Length).Max();

            _sb.Clear();

            _sb.AppendLine(string.Format("{0}\n{1}", title, new string('-', title.Length)));
            
            foreach (var (k, v) in keyValue)
            {
                _sb.AppendLine(string.Format("{0}: {1}", k.PadRight(maxKeyLength), v));
            }

            return _sb.ToString();
        }

        private static string MakeFieldString(object obj)
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

            return FormatString(obj.GetType().ToString(), _propertyMap);
        }

        public static void LogException(Exception e)
        {
            Debug.LogException(new Exception(MakeLogString(e.Message), e));
        }
    }