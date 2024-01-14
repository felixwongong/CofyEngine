using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace CofyEngine 
{
    public class SpriteAtlasManager: MonoInstance<SpriteAtlasManager>
    {
        public override bool persistent => true;
        
        private Dictionary<string, WeakReference<Sprite>> _spriteLoaded = new();
        private List<SpriteAtlas> _atlasLoaded = new();
        
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
                    _atlasLoaded.Add(atlas);
                    var tmpSprites = new Sprite[atlas.spriteCount];
                    atlas.GetSprites(tmpSprites);

                    foreach (var sprite in tmpSprites) 
                        _spriteLoaded.Add(sprite.name, new WeakReference<Sprite>(sprite));
                });
        }
    }
}