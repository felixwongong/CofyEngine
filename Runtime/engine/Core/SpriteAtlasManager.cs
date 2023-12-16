using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace CofyEngine 
{
    public class SpriteAtlasManager: MonoInstance<SpriteAtlasManager>
    {
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

        public void LoadAtlas(string path)
        {
            var absPath = path.concatPath(path);
            AssetManager.instance.LoadAsset<SpriteAtlas>(absPath)
                .OnSucceed(atlas =>
                {
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