namespace CofyEngine
{
    public class GamePreloadState: BaseState<BootStateId>
    {
        public override BootStateId id => BootStateId.GamePreload;
        protected internal override void StartContext(IStateMachine<BootStateId> sm, object param)
        {
            
        }
    }
}