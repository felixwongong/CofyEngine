using UnityEngine;

namespace CofyEngine.Util.Scriptable
{
    public abstract class CofyScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        public  void OnValidate()
        {
            Setup();
        }
#endif

        protected abstract void Setup();
        protected abstract void SetupLookUpTable();
    }
}