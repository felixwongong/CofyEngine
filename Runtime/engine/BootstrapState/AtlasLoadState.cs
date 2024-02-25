using System.Collections.Generic;
using UnityEngine.U2D;

namespace CofyEngine
{
    public class AtlasLoadState: IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.AtlasLoad;
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {

            var preloadAtlas = ConfigSO.inst.preloadAtlas;

            List<Future<SpriteAtlas>> loadFutures = SpriteAtlasManager.instance.LoadAtlas(preloadAtlas);

            var group = loadFutures.Count > 0 ?
                loadFutures.Group() :
                Future<List<SpriteAtlas>>.success(new List<SpriteAtlas>());
            
            LoadingUIPanel.instance.MonitorProgress(group, "Loading cute monster");
            
            group.OnSucceed(_ => { sm.GoToState(BootStateId.UI); });
        }

        public void OnEndContext()
        {
        }
    }
}