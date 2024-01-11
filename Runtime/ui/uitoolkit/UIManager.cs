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

        private Dictionary<Type, UIPanel> _panels = new();
        
        public Future<T> Bind<T>(T panel, BindingOption option = BindingOption.None) where T: UIPanel
        {
            _panels.Add(typeof(T), panel);
            Promise<T> bindingPromise = new Promise<T>();

            switch (option)
            {
                case BindingOption.None: bindingPromise.Resolve(panel); break;
                case BindingOption.Preload: 
                    panel.LoadAsset(false).Then(_ => bindingPromise.Resolve(panel));
                    break;
                case BindingOption.Clone:
                    panel.LoadAsset(true).Then(_ => bindingPromise.Resolve(panel));
                    break;
                default:
                    bindingPromise.Reject(new Exception(string.Format("BindingOption {0} not supported", option)));
                    break;
            }
            
            return bindingPromise.future;
        }
        
        public T GetPanel<T>() where T: UIPanel
        {
            return (T) _panels[typeof(T)];
        }
    }

    public enum BindingOption
    {
        None,
        Preload, 
        Clone
    }
}