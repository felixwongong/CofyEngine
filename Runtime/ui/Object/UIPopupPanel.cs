using UnityEngine;

namespace CofyUI
{
    public abstract class UIPopupPanel<TPanel>: UIInstance<TPanel>, IUIPanel
    {
        [SerializeField] protected Animator panelAnimator;
        
        public virtual void Show()
        {
        }

        public virtual void Hide()
        {
        }
    }
}