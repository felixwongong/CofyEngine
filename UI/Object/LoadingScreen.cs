using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using CofyUI;
using UnityEngine;

namespace CofyUI
{
    public class LoadingScreen : UIInstance<LoadingScreen>
    {
        [SerializeField] private ProgressBar bar;
        
        private IPromise target;

        private void Awake()
        {
            bar ??= GetComponentInChildren<ProgressBar>();
        }

        private void Update()
        {
            if (target != null)
            {
                bar.SetPercentage(target.progressFunc());    
                if (target.isCompleted) target = null;
            }
        }

        public void MonitorProgress(IPromise promise)
        {
            target = promise;
        }
    }
}