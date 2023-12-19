using System;
using System.Collections.Generic;
using CofyUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace CofyEngine
{
    public class UIManager: MonoInstance<UIRoot>
    {
        public override bool persistent => true;
        
        [SerializeField] private UIDocument _document;
        private Dictionary<string, UIScreen> _uiMap = new();

        protected override void Awake()
        {
            base.Awake();
            UIScreen.assetLoader = uxmlPath => AssetManager.instance.LoadAsset<VisualTreeAsset>(uxmlPath, AssetLoadOption.ForceLoadLocal);
        }

        public Future<T> Bind<T>(T screen, BindingOption option = BindingOption.None) where T: UIScreen
        {
            Promise<T> bindingPromise = new Promise<T>();

            if (_uiMap.ContainsKey(screen.uxmlPath))
                return Future<T>.failure(new Exception($"screen uxml path {screen.uxmlPath} already binded"));
            else _uiMap[screen.uxmlPath] = screen;

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

    public abstract class UIScreen
    {
        protected internal string uxmlPath;
        protected VisualTreeAsset asset;
        protected VisualElement root;

        public static Func<string, Future<VisualTreeAsset>> assetLoader;
        
        public UIScreen(string uxmlPath)
        {
            this.uxmlPath = uxmlPath;
        }

        public Future<bool> LoadAsset(bool instantiate)
        {
            if(assetLoader == null) return Future<bool>.failure(new NullReferenceException("assetLoader not set"));

            var assetLoadFuture = assetLoader(uxmlPath);

            if (!instantiate) return assetLoadFuture.TryMap(_ => true);
            
            return assetLoadFuture.TryMap(asset =>
            {
                this.asset = asset;
                root = asset.CloneTree();
                return true;
            });
        }
    }
}