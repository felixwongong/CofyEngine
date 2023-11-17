using System.Collections.Generic;
using CofyEngine;
using UnityEngine;

namespace CofyDev.RpgLegend
{
    [RequireComponent(typeof(Animator))]
    public class AnimationEventHandler : MonoBehaviour
    {
        private Animator _animator;
        
        private Dictionary<int, string> _hashToName = new Dictionary<int, string>();
        
        public SmartEvent<string> onAnimationEnd = new();
        public SmartEvent<string> onAnimationCallback = new();
        public SmartEvent<AnimationCommand> onAnimationCommand = new();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _animator.runtimeAnimatorController.animationClips.ForEach(clip =>
            {
                _hashToName.Add(Animator.StringToHash(clip.name), clip.name);
            });
            
            _animator.parameters.ForEach(param =>
            {
                _hashToName.Add(Animator.StringToHash(param.name), param.name);
            });
        }

        //Used for animation event only
        public void OnAnimationEvent(string message)
        {
            onAnimationCallback.Invoke(message);
        }

        public void OnAnimationCommand(AnimationCommand command)
        {
            onAnimationCommand.Invoke(command);    
        } 
        
        public void OnCurrentAnimationEnd()
        {
            var currentHash = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            onAnimationEnd?.Invoke(_hashToName[currentHash]);
        }
    }

    public enum AnimationCommand
    {
        SPAWN_VFX,
        STATE_TRIGGER
    }
}