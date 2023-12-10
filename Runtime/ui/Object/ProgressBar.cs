using System;
using UnityEngine;

namespace CofyUI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] 
        private GameObject occupying;
        
        [SerializeField] 
        [Range(0, 1)]
        private float _percentage;

        public void SetPercentage(float percent)
        {
            if (float.IsNaN(percent)) return;
            _percentage = percent;
            occupying.transform.localScale = new Vector3(Math.Clamp(percent, 0f, 1f), 1, 1);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetPercentage(_percentage);
        }
#endif
    }
}