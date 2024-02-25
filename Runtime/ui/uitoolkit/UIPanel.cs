using System.Diagnostics;
using System.Linq;
using UnityEngine.UIElements;

namespace CofyEngine
{
    public abstract class UIPanel 
    {
        private string uxmlPath;
        private VisualElement _root;

        public VisualElement root
        {
            get => _root;
            internal set => _root = value;
        }

        private bool _bShown = false;

        protected internal abstract void Construct(VisualElement root);

        public void Show()
        {
            UIManager.instance.LoadAsset(this);
            _bShown = true;
        }

        public void Hide()
        {
        }

        [Conditional("UNITY_EDITOR")]
        protected void validate(params VisualElement[] elements)
        {
            var notNull = elements.All(el => el != null);
            if (!notNull)
            {
                FLog.LogWarning("UIPanel {0} has null element", uxmlPath);
            }
        }
    }
}