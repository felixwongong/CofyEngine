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

        protected T Attach<T>(string name) where T : UIElement, new()
        {
            var el = new T();
            el.Construct(root.Q(name));
            return el;
        }

        [Conditional("UNITY_EDITOR")]
        protected void validate(params object[] elements)
        {
            var notNull = elements.All(el => el != null);
            if (!notNull)
            {
                FLog.LogWarning("UIPanel {0} has null element", uxmlPath);
            }
        }
    }
}