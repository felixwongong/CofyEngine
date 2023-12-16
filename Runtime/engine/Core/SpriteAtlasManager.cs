using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace CofyEngine 
{
    public class SpriteAtlasManager: MonoInstance<SpriteAtlasManager>
    {
        public override bool persistent => true;
        
        private Dictionary<string, SpriteAtlas> inAtlasMap = new(); //<spriteName, within Atlas>
        private void OnEnable()
        {
            UnityEngine.U2D.SpriteAtlasManager.atlasRegistered += onAtlasRegistered;
        }

        private void OnDisable()
        {
            UnityEngine.U2D.SpriteAtlasManager.atlasRegistered -= onAtlasRegistered;
        }

        private void onAtlasRegistered(SpriteAtlas atlas)
        { 
            FLog.Log(string.Format("atlas registered ({0})", atlas.name));   
        }

        public Future<SpriteAtlas> LoadAtlas(string path)
        {
            return AssetManager.instance.LoadAsset<SpriteAtlas>(path)
                .Then(future =>
                {
                    var atlas = future.result;
                    var tmpSprites = new Sprite[atlas.spriteCount];
                    atlas.GetSprites(tmpSprites);
                    for (var i = 0; i < tmpSprites.Length; i++)
                    {
                        inAtlasMap.Add(tmpSprites[i].name, atlas);
                    }
                });
        }
    }
}