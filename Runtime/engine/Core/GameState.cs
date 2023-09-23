using CofyUI;

namespace CofyEngine
{
    public abstract class GameState: IPromiseState
    {
        protected abstract string scene { get;  }
        protected abstract IUIPanel uiPanel { get; }
        
        void IPromiseState.StartContext(IPromiseSM sm)
        {
            if(scene.notNullOrEmpty()) LevelManager.instance.LoadLevel(scene);

            uiPanel?.ShowPanel(true);
        }
    }
}