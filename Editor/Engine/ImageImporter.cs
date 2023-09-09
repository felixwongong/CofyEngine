using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CofyEngine.Editor
{
    public class ImageImporter : EditorWindow
    {
        private Texture2D previewTexture;
        private bool isProcessing;

        [MenuItem("Cofy Tools/Image Importer")]
        private static void ShowWindow()
        {
            var window = GetWindow<ImageImporter>();
            window.titleContent = new GUIContent("Image Importer");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Import Image", EditorStyles.boldLabel);

            GUILayout.Space(10);

            if (GUILayout.Button("Select Image"))
            {
                var jfifPath = EditorUtility.OpenFilePanel("Select Image", "", "jfif,png,jpg");
                if (!string.IsNullOrEmpty(jfifPath))
                {
                    LoadPreviewTexture(jfifPath);
                }
            }

            GUILayout.Space(10);

            if (previewTexture != null)
            {
                GUILayout.Label(previewTexture, new GUILayoutOption[]
                {
                    GUILayout.MaxHeight(200)
                });
            }

            GUILayout.Space(10);

            EditorGUI.BeginDisabledGroup(isProcessing);
            if (GUILayout.Button("Import as Sprite"))
            {
                ImportJfifAsSprite();
            }

            EditorGUI.EndDisabledGroup();
        }

        private void LoadPreviewTexture(string jfifPath)
        {
            var texturePromise = LoadTextureFromPath(jfifPath);
            texturePromise.Then(texture =>
            {
                previewTexture = texture.result;
                Repaint();
            });
        }

        private void ImportJfifAsSprite()
        {
            string savePath = EditorUtility.SaveFilePanel("Save as Sprite", "Assets", "sprite", "png");
            if (!string.IsNullOrEmpty(savePath))
            {
                byte[] pngBytes = previewTexture.EncodeToPNG();
                File.WriteAllBytes(savePath, pngBytes);
                string relativePath = "Assets" + savePath.Substring(Application.dataPath.Length);
                AssetDatabase.ImportAsset(relativePath, ImportAssetOptions.ForceUpdate);
                TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(relativePath);
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.SaveAndReimport();
                }
            }
        }

        private Future<Texture2D> LoadTextureFromPath(string path)
        {
            string url = new System.Uri(path).AbsoluteUri;

            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);

            Promise<Texture2D> texturePromise = new Promise<Texture2D>(() => webRequest.downloadProgress);

            webRequest.SendWebRequest().Future()
                .Then(op =>
                {
                    if (webRequest.result == UnityWebRequest.Result.Success)
                        texturePromise.Resolve(DownloadHandlerTexture.GetContent(webRequest));
                    else
                        texturePromise.Reject($"Image on path ({path}) unable to be loaded");
                })
                .Then(op => webRequest.Dispose());

            return texturePromise.future;
        }
    }
}