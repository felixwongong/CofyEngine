using System;
using CofyEngine.Runtime.Engine.Util;
using UnityEngine;

namespace CofyUI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private GameObject occupying;

        public void SetPercentage(float percent)
        {
            occupying.transform.localScale = new Vector3(Math.Clamp(percent, 0f, 1f), 1, 1);
        }
    }
}