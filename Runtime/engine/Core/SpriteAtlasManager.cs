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

        public static Func<string, Future<SpriteAtlas>> atlasLoader;
        
        private void OnEnable()
        {
            UnityEngine.U2D.SpriteAtlasManager.atlasRequested += onAtlasRequested;
            UnityEngine.U2D.SpriteAtlasManager.atlasRegistered += onAtlasRegistered;
        }

        private void OnDisable()
        {
            UnityEngine.U2D.SpriteAtlasManager.atlasRequested -= onAtlasRequested;
            UnityEngine.U2D.SpriteAtlasManager.atlasRegistered -= onAtlasRegistered;
        }

        private void onAtlasRegistered(SpriteAtlas atlas)
        { 
            FLog.Log(string.Format("atlas registered ({0})", atlas.name));   
        }
        
        private void onAtlasRequested(string atlasName, Action<SpriteAtlas> res)
        {
            FLog.Log(string.Format("atlas requested ({0})", atlasName));
            var future = LoadAtlas(atlasName);
            future.OnSucceed(res);
        }

        public Future<SpriteAtlas> LoadAtlas(string atlasName)
        {
            return atlasLoader(atlasName)
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

        public List<Future<SpriteAtlas>> LoadAtlas(List<string> atlasNames)
        {
            var futures = new List<Future<SpriteAtlas>>();
            foreach (var atlasName in atlasNames)
            {
                futures.Add(LoadAtlas(atlasName));
            }

            return futures;
        }
    }
}