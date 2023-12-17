using UnityEngine;

namespace CofyUI
{
    public class UIInstance<T>: MonoBehaviour
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                _instance ??= UIRoot.instance.GetUI<T>();
                return _instance;
            }
        }
    }

    public interface IUIPanel
    {
        public void Show();
        public void Hide();
    }
}