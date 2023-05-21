using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace cofydev.util
{
    public static class FLog
    {
        public static void Log(object inMsg, object inObj = null)
        {
            Debug.Log(MakeLogString(inMsg.ToString(), inObj));
        }

        public static void LogWarning(object inMsg, object inObj = null)
        {
            Debug.LogWarning(MakeLogString(inMsg.ToString(), inObj));
        }

        public static void LogError(object inMsg, object inObj = null)
        {
            Debug.LogError(MakeLogString(inMsg.ToString(), inObj));
        }
        
        private static string MakeLogString(string inMsg = "", object inObj = null)
        {
            
            var method = new StackTrace().GetFrame(2).GetMethod();
            Type type = method.DeclaringType;
            
            try
            {
                var message = string.IsNullOrEmpty(inMsg) ? "" : $"{inMsg}\n";
                var obj = inObj == null ? "" : MakeFieldString(inObj);
                while (type?.DeclaringType != null)
                {
                    type = type?.DeclaringType;
                }

                return $"[{type?.Name}]: {message}{obj}";

            }
            catch (Exception e)
            {
                return $"[{type?.Name}]: {e.ToString()}";
            }
        }
         
        public static string FormatString(string title, Dictionary<string, string> keyValue)
        {
            int maxKeyLength = keyValue.Keys.Select(k => k.Length).Max();

            StringBuilder sb = new StringBuilder();
            
            foreach (var (k, v) in keyValue)
            {
                sb.AppendLine($"{k.PadRight(maxKeyLength)}: {v}");
            }

            sb.Insert(0, $"\n{StrRepeat("-", title.Length)}\n");
            sb.Insert(0, title);

            return sb.ToString();
        }

        public static string MakeFieldString(object obj)
        {
            var propertyMap = new Dictionary<string, string>();
            
            //Handling struct/class object
            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var fieldInfo in fields)
            {
                var val = fieldInfo.GetValue(obj);
                if(val == null) continue;
                propertyMap[fieldInfo.Name] = fieldInfo.GetValue(obj).ToString();
            }

            return FormatString(obj.GetType().ToString(), propertyMap);
        }

        public static String StrRepeat(string s, int time)
        {
            return String.Concat(Enumerable.Repeat(s, time));
        }

        public static void LogException(Exception e)
        {
            throw new Exception(MakeLogString(e.Message), e);
        }
    }
}