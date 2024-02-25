using CofyUI;

namespace CofyEngine
{
    public abstract class GameState: IPromiseState<GameStateId>
    {
        protected abstract string scene { get;  }
        public abstract GameStateId id { get; }
        
        public void StartContext(IPromiseSM<GameStateId> sm, object param)
        {
            if(scene.notNullOrEmpty()) LevelManager.instance.LoadLevel(scene);
        }

        public void OnEndContext()
        {
        }
    }
}