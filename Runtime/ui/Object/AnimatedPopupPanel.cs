using UnityEngine;

namespace CofyUI
{
    [ExecuteAlways]
    //TODO: use name hash, add more editor param checking to ensure dun get wrong on designer
    public abstract class AnimatedPopupPanel<TPanel>: UIInstance<TPanel>, IUIPanel
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected string showAnim = "show_panel";
        [SerializeField] protected string hideAnim = "hide_panel";

        public virtual void Show()
        {
            _animator.Play(showAnim);
        }

        public virtual void Hide()
        {
            _animator.Play(hideAnim);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }
#endif
    }
}