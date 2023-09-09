using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CofyEngine.Engine.Util
{
    public static class Extension
    {
        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }

        public static IEnumerator ToRoutine(this Task t)
        {
            while (!t.IsCompleted)
            {
                yield return null;
            }

            if (!t.IsCompletedSuccessfully)
            {
                FLog.LogError($"Task ({t}) failed or canceled!", t.Exception);
            }
        }

        public static void DisableAllChildren(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public static void DestroyAllChildren(this Transform transform, bool immediate = false)
        {
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            for (int i = 0; i < children.Length; i++)
            {
                if (!immediate) Object.Destroy(children[i].gameObject);
                else Object.DestroyImmediate(children[i].gameObject);
            }
        }

        public static bool NotKeyedOrNull<A, B>(this Dictionary<A, B> dictionary, A key)
        {
            return !dictionary.ContainsKey(key) || dictionary[key] == null;
        }

        public static void ForEach<T>(this IList<T> loopable, Action<T> action)
        {
            for (var i = 0; i < loopable.Count; i++)
            {
                action(loopable[i]);
            }
        }

        public static T FindInParent<T>(this Transform transform)
        {
            return transform.parent != null ? transform.parent.GetComponentInParent<T>(true) : default;
        }

        public static void SetGoActive(this MonoBehaviour mono, bool active)
        {
            mono.gameObject.SetActive(active);
        }

        public static Sprite ToSprite(this Texture2D texture)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            // Get the texture width and height
            int width = texture.width;
            int height = texture.height;

            // Create a Rect object with the texture dimensions
            Rect rect = new Rect(0, 0, width, height);

            // Create a Sprite from the Texture2D
            Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

            return sprite;
        }

        public static double CalculateDistance(this Vector2 latlng1, Vector2 latlng2) 
        {
            double R = 6371e3; // Earth's radius in meters
            double lat1Rad = ToRadians(latlng1.x);
            double lat2Rad = ToRadians(latlng2.x);
            double deltaLat = ToRadians(latlng2.x - latlng1.x);
            double deltaLng = ToRadians(latlng2.y - latlng1.y);

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLng / 2) * Math.Sin(deltaLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        public static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static string GetTName(this object obj)
        {
            return obj?.GetType().Name;
        }
    }

    public static class JsonUtil
    {
        public static T Deserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}