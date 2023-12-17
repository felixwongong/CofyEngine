using UnityEngine;

namespace CofyUI
{
    public abstract class UIPopupPanel<TPanel>: UIInstance<TPanel>, IUIPanel
    {
        [SerializeField] protected Animator _animator;
        
        public virtual void Show()
        {
        }

        public virtual void Hide()
        {
        }
    }
}