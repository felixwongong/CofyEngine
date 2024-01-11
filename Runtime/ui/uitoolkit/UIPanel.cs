using System;
using System.Diagnostics;
using System.Linq;
using CofyUI;
using UnityEngine.UIElements;

namespace CofyEngine
{
    public abstract class UIPanel: IUIPanel
    {
        private string uxmlPath;
        private VisualElement root;
        private VisualElement _parent;

        private Future<VisualTreeAsset> _assetLoadFuture;

        private bool _bAddedToRoot = false;
        private bool _bShown = false;

        public virtual string showTransition => "show_panel";
        public virtual string hideTransition => "hide_panel";
        public static Func<string, Future<VisualTreeAsset>> assetLoader;
        
        public UIPanel(string uxmlPath)
        {
            this.uxmlPath = uxmlPath;
        }

        //use for direct inject asset instead of loading from asset loader, use-case: local load ui
        public UIPanel(VisualTreeAsset asset)
        {
            this._assetLoadFuture = Future<VisualTreeAsset>.success(asset);
        }

        protected abstract void Construct(VisualElement root);

        //Should have a much cleaner way to early exit using future hooks, but this is just easier to read
        public Future<bool> LoadAsset(bool instantiate)
        {
            if (assetLoader == null) return Future<bool>.failure(new NullReferenceException("assetLoader not set"));

            if (instantiate && root != null) return Future<bool>.success(true);

            if (!instantiate && _assetLoadFuture is { isSucceed: true })
                return Future<bool>.success(true);

            return _LoadAsset(instantiate);
        }

        private Future<bool> _LoadAsset(bool instantiate)
        {
            _assetLoadFuture ??= assetLoader(uxmlPath);
            if (!instantiate) return _assetLoadFuture.TryMap(asset => true);

            return _assetLoadFuture.TryMap(asset =>
            {
                root = asset.CloneTree();
                Construct(root);
                return true;
            });
        }

        public void Show()
        {
            LoadAsset(true).OnSucceed(future =>
            {
                if (!_bAddedToRoot)
                {
                    UIManager.instance.doc.rootVisualElement.Add(root);
                    root.AddToClassList("panel");
                    _bAddedToRoot = true;
                }
            });
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