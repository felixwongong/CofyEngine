using System;
using System.Collections.Generic;
using CofyEngine;
using Unity.Mathematics;
using UnityEngine;

namespace CofyDev.RpgLegend
{
    [DefaultExecutionOrder(-10)]
    public class CofyAnimator: MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private Animator _animator;

        [SerializeField] private AnimationEventHandler _eventHandler;

        public AnimationEventHandler eventHandler {
            get
            {
                _eventHandler ??= _animator.GetOrAddComponent<AnimationEventHandler>();
                return _eventHandler;
            }
        }

        Dictionary<string, int> _nameToHash = new ();
        
        private void Awake()
        {
            if(!_animator) _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _animator.runtimeAnimatorController.animationClips.ForEach(clip =>
            {
                _nameToHash.Add(clip.name, Animator.StringToHash(clip.name));
            });
            
            _animator.parameters.ForEach(param =>
            {
                _nameToHash.Add(param.name, Animator.StringToHash(param.name));
            });
        }

        public void SetFloat01(string paramName, float value)
        {
            _animator.SetFloat(getHash(paramName), math.clamp(value, 0, 1));
        }

        public void SetTrigger(string paramName)
        {
            _animator.SetTrigger(getHash(paramName));
        }

        public void PlayAnim(string animName)
        {
            _animator.Play(getHash(animName));
        }
        
        public void PlayState(string stateName)
        {
            _animator.Play(stateName);
        }

        private int getHash(string animName)
        {
            if (!_nameToHash.TryGetValue(animName, out var hash))
            {
                throw new Exception(string.Format("No Animation Registered: {0}", animName));
            }

            return hash;
        }
    }
}