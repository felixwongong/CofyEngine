﻿using UnityEngine;

namespace CofyEngine
{
    public class UIInstance<T>: MonoBehaviour
    {
        public static T instance { get; set; }
    }
}