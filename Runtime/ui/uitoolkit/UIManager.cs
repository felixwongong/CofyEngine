using System.Collections.Generic;
using CofyUI;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

namespace CofyEngine
{
    public class UIManager: MonoInstance<UIRoot>
    {
        public override bool persistent => true;
        
        [SerializeField] private UIDocument _document;
        private Dictionary<System.Type, UIScreen> _uiMap = new();

        public Future<T> Bind<T>(T screen, BindingOption option = BindingOption.None) where T: UIScreen
        {
            Promise<T> bindingPromise = new Promise<T>();
            
            return bindingPromise.future;
        }

        public void Test()
        {
            
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
        protected VisualElement screenRoot;

        public static Func<VisualTreeAsset, string> loadAsset;
        
        public UIScreen(string uxmlPath)
        {
            this.uxmlPath = uxmlPath;
        }
    }
}