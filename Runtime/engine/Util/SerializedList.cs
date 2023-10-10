using System;
using System.Collections.Generic;
using UnityEngine;

namespace CofyEngine.Util
{
    [Serializable]
    public class SerializedList<T>
    {
        [SerializeField]
        public List<T> list = new List<T>();
    }
}