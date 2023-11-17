using System.Collections.Generic;
using CofyEngine.Util;
using UnityEngine;

namespace CofyDev.RpgLegend
{
    [CreateAssetMenu(fileName = "AnimationListVariable", menuName = "CofyDev/AnimationList")]
    public class AnimationListVariable : ScriptableObject
    {
        public List<AnimationVariable> animationList;
    }

    [CreateAssetMenu(fileName = "AnimationVariable", menuName = "CofyDev/Animation")]
    public class AnimationVariable : ScriptableObject
    {
        public Animation animation;
    }
    
    [CreateAssetMenu(fileName = "AnimatorVariable", menuName = "CofyDev/Animator")]
    public class AnimatorVariable : ScriptableObject
    {
        public Animator animator;

        private Dictionary<string, int> _nameToHash;
    }
}