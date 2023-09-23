using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CofyEngine.Engine.Util.UI
{
    //TODO: Add image loading
    public class AsyncImageBinder : UIBinder<Image>
    {
        public State<string> url = new();

        private Coroutine downloadRoutine;
        private UnityWebRequest req;
        
        private void OnEnable()
        {
            url.onChange += OnUrlChange;    
        }

        private void OnDisable()
        {
            url.onChange -= OnUrlChange;    
        }

        private void Start()
        {
            OnUrlChange(url);
        }

        private void OnUrlChange(string newUrl)
        {
            if(string.IsNullOrEmpty(newUrl)) return;
            if(req != null) req.Dispose();
            if(downloadRoutine != null)StopCoroutine(downloadRoutine);

            downloadRoutine = StartCoroutine(DownloadImage(newUrl));
        }
        
        IEnumerator DownloadImage(string url)
        {
            req = UnityWebRequestTexture.GetTexture(url);
            req.disposeDownloadHandlerOnDispose = true;
            req.useHttpContinue = true;
            req.downloadHandler = new DownloadHandlerTexture(true);
            var op = req.SendWebRequest();
            yield return op;

            while (!op.isDone || req.result == UnityWebRequest.Result.InProgress)
            {
                FLog.Log($"Waiting download {url}");
                yield return null;
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                FLog.LogError($"image download from {url} failed, {req.result}");
                yield break;
            }

            var texture = DownloadHandlerTexture.GetContent(req);
            if (texture != null)
            {
                target.sprite = texture.ToSprite();
            }
            
            req.Dispose();
        }
    }
}