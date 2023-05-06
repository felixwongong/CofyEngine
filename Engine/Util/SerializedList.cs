using System;
using System.Collections.Generic;
using UnityEngine;

namespace cofydev.util
{
    [Serializable]
    public class SerializedList<T>
    {
        [SerializeField]
        public List<T> list = new List<T>();
    }
}