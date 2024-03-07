using CofyUI;

namespace CofyEngine
{
    public abstract class GameState: IState<GameStateId>
    {
        protected abstract string scene { get;  }
        public abstract GameStateId id { get; }
        
        public void StartContext(IStateMachine<GameStateId> sm, object param)
        {
            if(scene.notNullOrEmpty()) LevelManager.instance.LoadLevel(scene);
        }

        public void OnEndContext()
        {
        }
    }
}