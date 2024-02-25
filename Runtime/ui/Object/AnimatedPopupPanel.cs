using System;
using CofyEngine;
using UnityEngine;

namespace CofyUI
{
    //TODO: use name hash, add more editor param checking to ensure dun get wrong on designer
    public abstract class AnimatedPopupPanel<TPanel>: UIInstance<TPanel>
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected string showAnim = "show_panel";
        [SerializeField] protected string hideAnim = "hide_panel";

        #region state string hashing

        //TODO: add caching
        private int showHash => Animator.StringToHash(showAnim);
        private int hideHash => Animator.StringToHash(hideAnim);

        #endregion

        #pragma warning disable 0414 
        //Use reg cuz im lazy, can use enable disable for event handling instead
        private IRegistration animEndReg;
        #pragma warning restore 0414
        
        private void Start()
        {
            animEndReg = _animator.GetBehaviour<AnimationBehaviour>().onExit.Register(OnHideAnimEnd);
        }

        private void OnHideAnimEnd(AnimatorStateInfo info)
        {
            if (info.shortNameHash == hideHash) onHidePanel();
        }

        public virtual void Show()
        {
            _animator.Play(showHash);
        }

        public virtual void Hide()
        {
            _animator.Play(hideHash);
        }
        
        protected virtual void onHidePanel()
        {
            gameObject.SetActive(false);
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