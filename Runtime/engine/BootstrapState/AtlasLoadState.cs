using System.Collections.Generic;
using CofyUI;
using UnityEngine.U2D;

namespace CofyEngine
{
    public class AtlasLoadState: IPromiseState<BootStateId>
    {
        public BootStateId id => BootStateId.AtlasLoad;
        public void StartContext(IPromiseSM<BootStateId> sm, object param)
        {
            List<Future<SpriteAtlas>> loadFutures = new();

            var preloadAtlas = ConfigSO.inst.preloadAtlas;
            var atlasDirectory = ConfigSO.inst.atlasDirectory;
            
            for (var i = 0; i < preloadAtlas.Count; i++)
            {
                var path = atlasDirectory.concatPath(preloadAtlas[i]);
                loadFutures.Add(SpriteAtlasManager.instance.LoadAtlas(path));
            }

            var group = loadFutures.Group();
            LoadingScreen.instance.MonitorProgress(group, "Loading cute monster");
            
            group.OnSucceed(_ =>
            {
                sm.GoToState(BootStateId.UI);
            });
        }

        public void OnEndContext()
        {
        }
    }
}