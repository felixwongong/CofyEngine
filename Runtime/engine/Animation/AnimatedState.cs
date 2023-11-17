using System;
using CofyEngine;
using UnityEngine;

namespace CofyDev.RpgLegend
{
    public abstract class AnimatedState : MonoBehaviour, IPromiseState
    {
        protected CofyAnimator animator;

        private IRegistration _eventReg;
        private IRegistration _endReg;
        
        protected virtual void Awake()
        {
            if (!animator) animator = GetComponentInChildren<CofyAnimator>();
        }

        private void OnDisable()
        {
            _eventReg = null;
            _endReg = null;
        }

        public abstract void StartContext(IPromiseSM sm, object param);

        public virtual void OnEndContext(){}
        
        protected IRegistration RegisterAnimationEndOnce(string animName, Action callback)
        {
            _endReg = animator.eventHandler.onAnimationEnd.RegisterOnce(ended =>
            {
                if (ended == animName)
                {
                    callback();
                }
            });
            return _endReg;
        }
        
        protected void RegisterAnimationCommand(Action<AnimationCommand> callback)
        {
            _eventReg = animator.eventHandler.onAnimationCommand.Register(callback);
        }
    }
}