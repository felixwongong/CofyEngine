using CofyUI;

namespace CofyEngine
{
    public abstract class GameState: IPromiseState<GameStateId>
    {
        protected abstract string scene { get;  }
        protected abstract IUIPanel uiPanel { get; }
        public abstract GameStateId id { get; }
        
        public void StartContext(IPromiseSM<GameStateId> sm, object param)
        {
            if(scene.notNullOrEmpty()) LevelManager.instance.LoadLevel(scene);

            uiPanel?.ShowPanel(true);

        }

        public void OnEndContext()
        {
        }
    }
}