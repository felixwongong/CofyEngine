using System;
using CofyEngine.Engine.Util;
using UnityEngine;

namespace CofyUI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private GameObject occupying;

        public void SetPercentage(float percent)
        {
            if (float.IsNaN(percent)) return;
            occupying.transform.localScale = new Vector3(Math.Clamp(percent, 0f, 1f), 1, 1);
        }
    }
}