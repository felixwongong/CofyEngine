using CofyEngine;
using TMPro;
using UnityEngine;

namespace CofyUI
{
    public class LoadingScreen : UIInstance<LoadingScreen>
    {
        [SerializeField] private ProgressBar bar;
        [SerializeField] private TextMeshProUGUI text;
        
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

        public void MonitorProgress(IFuture future, string message = "")
        {
            this.SetGoActive(true);
            target = future;
            if (message != null && message.notNullOrEmpty()) 
                text.text = message;
        }

        public void EndMonitoring()
        {
            target = null;
            this.SetGoActive(false);
        }
    }
}
