using CofyEngine.Core;
using UnityEngine.UIElements;

namespace CofyEngine
{
    public class LoadingUIPanel: UIPanel
    {
        private TextElement message;
        private ProgressBar progress;
        
        //State
        private IFuture target;
        
        protected override void Construct(VisualElement root)
        {
            message = root.Q<TextElement>("#message");
            progress = root.Q<ProgressBar>("#progress");
            validate(message, progress);
            
            progress.value = 0;

            MainThreadExecutor.instance.QueueFrameAction(OnUpdate);
        }
        
        public void MonitorProgress(IFuture future, string message = "")
        {
            target = future;
            if (message != null && message.notNullOrEmpty()) 
                this.message.text = message;
        }

        private void OnUpdate()
        {
            if (target == null) return;
            
            progress.value = target.progress;
            if (target.isCompleted)
            {
                target = null;
            }
        }

        public LoadingUIPanel(string uxmlPath) : base(uxmlPath) { }
        public LoadingUIPanel(VisualTreeAsset asset) : base(asset) { }
    }
}