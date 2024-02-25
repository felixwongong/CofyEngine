using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace CofyEngine
{
    public class UIManager: MonoInstance<UIManager>
    {
        public override bool persistent => true;
        
        [FormerlySerializedAs("_document")] 
        [SerializeField] protected internal UIDocument doc;

        private Dictionary<Type, Triplet<UIPanel, string, Future<VisualTreeAsset>>> _panels = new();

        public static Func<string, Future<VisualTreeAsset>> assetLoader;
        
        public Future<T> Bind<T>(T panel, string uxmlPath, BindingOption option = BindingOption.None) where T: UIPanel
        {
            _panels.Add(typeof(T), new Triplet<UIPanel,string, Future<VisualTreeAsset>>()
            {
                a = panel,
                b = uxmlPath,
                c = null
            });
            
            var bindingPromise = new Promise<T>();

            switch (option)
            {
                case BindingOption.None: bindingPromise.Resolve(panel); break;
                case BindingOption.CreateInstance:
                    LoadAsset(panel).Then(_ => bindingPromise.Resolve(panel));
                    break;
                default:
                    bindingPromise.Reject(new Exception(string.Format("BindingOption {0} not supported", option)));
                    break;
            }
            
            return bindingPromise.future;
        }

        public Future<bool> LoadAsset(UIPanel panel)
        {
            if (assetLoader == null) return Future<bool>.failure(new NullReferenceException("assetLoader not set"));
            
            if (panel.root != null) return Future<bool>.success(true);
           
            return _LoadAsset(panel).TryMap(asset => true);
        }
        
        private Future<VisualTreeAsset> _LoadAsset(UIPanel panel)
        {
            var panelProperty = _panels[panel.GetType()];
            if (panelProperty.c != null) return panelProperty.c;
            
            panelProperty.c = assetLoader(panelProperty.b);

            panelProperty.c.OnSucceed(asset =>
            {
                panel.root = asset.CloneTree();
                panel.Construct(panel.root);
                
                doc.rootVisualElement.Add(panel.root);
                panel.root.AddToClassList("panel");
            });
            
            return panelProperty.c;
        }
        
        public T GetPanel<T>() where T: UIPanel
        {
            return (T) _panels[typeof(T)].a;
        }
    }
    
    public class Pair<A, B>
    {
        public A a;
        public B b;
    }

    public class Triplet<A, B, C>
    {
        public A a;
        public B b;
        public C c;
    }
    
    public enum BindingOption
    {
        None,
        CreateInstance
    }
}