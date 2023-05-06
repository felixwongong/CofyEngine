using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CM.Network.Auth;
using CM.Util.Singleton;
using cofydev.util;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

namespace CM.Network.CloudSave
{
    public class FirestoreWrapper : SingleBehaviour<FirestoreWrapper>
    {
        public string userId => FirebaseAuth.DefaultInstance.CurrentUser?.Email;
        public bool hasUser => !string.IsNullOrEmpty(userId);

        private FirebaseFirestore db;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => FirebaseAuthHelper.Singleton.isFirebaseReady);
            db = FirebaseFirestore.DefaultInstance;
            FLog.Log($"{db}");
        }

        public async Task SavePlayerData(Dictionary<string, object> data)
        {
            if (string.IsNullOrEmpty(userId))
            {
                FLog.LogWarning("User ID not valid, cannot save.");
                return;
            }

            try
            {
                foreach (var (k, v) in data)
                {
                    DocumentReference targetDoc = db.Collection(k).Document(userId);
                    if (v == null)
                    {
                        await targetDoc.DeleteAsync();
                        FLog.Log($"Deleted data, collection ({k}), document: ({userId})", v);
                    }
                    else
                    {
                        await targetDoc.SetAsync(v);
                        FLog.Log($"Saved user data in collection ({k})", v);
                    }
                }
            }
            catch (Exception e)
            {
                //TODO: Fix NullReferenceException in tutorial scene save
                FLog.LogException(e);
            }
        }

        public async Task SaveData(Dictionary<KeyPair, object> data)
        {
            try
            {
                foreach (var (kp, v) in data)
                {
                    DocumentReference targetDoc = db.Collection(kp.collectionName).Document(kp.documentName);
                    if (v == null)
                    {
                        await targetDoc.DeleteAsync();
                        FLog.Log($"Deleted data, collection ({kp.collectionName}), document: ({kp.documentName})", v);
                    }
                    else
                    {
                        await targetDoc.SetAsync(v);
                        FLog.Log($"Saved data, collection ({kp.collectionName}), document: ({kp.documentName})", v);
                    }
                }
            }
            catch (Exception e)
            {
                FLog.LogException(e);
            }
        }

        public async Task RemoveUserCollection()
        {
            var fields = typeof(SaveKey).GetFields();
            foreach (var field in fields)
            {
                var value = field.GetValue(null) as string;
                var doc = db.Collection(value).Document(userId);
                await doc.DeleteAsync();
            }

            FLog.Log("Firebase documents deleted.");
        }

        private async Task<DocumentSnapshot> GetPlayerSnapshot(string key)
        {
            return await GetSnapshot(new KeyPair(key, userId));
        }

        private async Task<DocumentSnapshot> GetSnapshot(KeyPair kp)
        {
            FLog.Log($"Getting data ({kp.documentName}) from collection ({kp.collectionName})");

            var targetDoc = db.Collection(kp.collectionName).Document(kp.documentName);
            FLog.Log($"docId: ({targetDoc.Id}), userid: {userId}");
            return await targetDoc.GetSnapshotAsync();
        }

        public async Task<T> GetPlayerOne<T>(string key)
        {
            try
            {
                if (!hasUser) return default;
                var snapshot = await GetPlayerSnapshot(key);
                if (snapshot.Exists)
                {
                    
                    return snapshot.ConvertTo<T>();
                }
                return default;
            }
            catch (Exception e)
            {
                FLog.LogException(e);
            }

            return default;
        }

        public async Task<string> GetPlayerOneJson<T>(string key)
        {

            T result = await GetPlayerOne<T>(key);
            var json = JsonUtility.ToJson(result);

            return json;
        }

        public async Task<T> GetOne<T>(KeyPair kp)
        {
            try
            {
                if (!hasUser) return default;
                var snapshot = await GetSnapshot(kp);
                if (snapshot.Exists)
                {
                    
                    return snapshot.ConvertTo<T>();
                }
                return default;
            }
            catch (Exception e)
            {
                FLog.LogException(e);
            }

            return default;
        }
        public async Task<string> GetOneJson<T>(KeyPair kp)
        {
            T result = await GetOne<T>(kp);
            return JsonUtility.ToJson(result);
        }
    }

    public class KeyPair
    {
        public readonly string collectionName;
        public readonly string documentName;

        public KeyPair(string colName, string docName)
        {
            this.collectionName = colName;
            this.documentName = docName;
        }

        public override bool Equals(object obj)
        {
            if (obj is KeyPair kp)
            {
                return kp.collectionName == collectionName && kp.documentName == collectionName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(collectionName, documentName);
        }
    }
}