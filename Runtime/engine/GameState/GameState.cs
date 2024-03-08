using CofyUI;

namespace CofyEngine
{
    public abstract class GameState: BaseState<GameStateId>
    {
        protected abstract string scene { get;  }
        public abstract override GameStateId id { get; }

        protected internal override void StartContext(IStateMachine<GameStateId> sm, object param)
        {
            if(scene.notNullOrEmpty()) LevelManager.instance.LoadLevel(scene);
        }
    }
}