using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CofyUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace CofyEngine
{
    public class UIManager: MonoInstance<UIRoot>
    {
        public override bool persistent => true;
        
        [SerializeField] private UIDocument _document;

        protected override void Awake()
        {
            base.Awake();
            UIPanel.assetLoader = uxmlPath => AssetManager.instance.LoadAsset<VisualTreeAsset>(uxmlPath);
            var asset = Resources.Load<VisualTreeAsset>("toolkit_loading_ui_panel");
            var loading = new LoadingUIPanel(asset);
            Bind(loading, BindingOption.Clone);
            _document.rootVisualElement.Add(loading.root);
        }

        public Future<T> Bind<T>(T screen, BindingOption option = BindingOption.None) where T: UIPanel
        {
            Promise<T> bindingPromise = new Promise<T>();

            switch (option)
            {
                case BindingOption.None: bindingPromise.Resolve(screen); break;
                case BindingOption.Preload: 
                    screen.LoadAsset(false).Then(_ => bindingPromise.Resolve(screen));
                    break;
                case BindingOption.Clone:
                    screen.LoadAsset(true).Then(_ => bindingPromise.Resolve(screen));
                    break;
                default:
                    bindingPromise.Reject(new Exception(string.Format("BindingOption {0} not supported", option)));
                    break;
            }
            
            return bindingPromise.future;
        }
    }

    public enum BindingOption
    {
        None,
        Preload, 
        Clone
    }

    public abstract class UIPanel
    {
        protected internal string uxmlPath;
        protected internal VisualElement root;

        private Future<VisualTreeAsset> _assetLoadFuture;

        public static Func<string, Future<VisualTreeAsset>> assetLoader;
        
        public UIPanel(string uxmlPath)
        {
            this.uxmlPath = uxmlPath;
        }
        
        //use for direct inject asset instead of loading from asset loader
        public UIPanel(VisualTreeAsset asset)
        {
            this._assetLoadFuture = Future<VisualTreeAsset>.success(asset);
        }

        protected abstract void Construct(VisualElement root);

        //Should have a much cleaner way to early exit using future hooks, but this is just easier to read
        public Future<bool> LoadAsset(bool instantiate)
        {
            if(assetLoader == null) return Future<bool>.failure(new NullReferenceException("assetLoader not set"));

            if (instantiate && root != null) return Future<bool>.success(true);
            
            if(!instantiate && _assetLoadFuture is { isSucceed: true })
                return Future<bool>.success(true);;

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