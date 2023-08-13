using UnityEngine;

namespace CofyEngine.Engine.Core
{
    public abstract class GameState: MonoBehaviour, IPromiseState
    {
        protected abstract string scene { get;  }
        
        void IPromiseState.StartContext(IPromiseSM sm)
        {
            if(scene.notNullOrEmpty()) LevelManager.instance.LoadLevelFull(scene);
        }
    }
}