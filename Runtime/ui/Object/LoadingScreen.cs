using CofyEngine.Engine.Util;
using UnityEngine;

namespace CofyUI
{
    public class LoadingScreen : UIInstance<LoadingScreen>
    {
        [SerializeField] private ProgressBar bar;
        
        private IFuture target;

        private void Awake()
        {
            bar ??= GetComponentInChildren<ProgressBar>();
        }

        private void Update()
        {
            if (target != null && bar != null)
            {
                bar.SetPercentage(target.progress);
                if (target.isCompleted)
                {
                    target = null;
                }
            }
        }

        public void MonitorProgress(IFuture future)
        {
            this.SetGoActive(true);
            target = future;
        }

        public void EndMonitoring()
        {
            target = null;
            this.SetGoActive(false);
        }
    }
}
