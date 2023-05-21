using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using cofydev.util;
using Unity.Services.CloudSave;
using UnityEngine;

namespace CM.Network.CloudSave
{
    public class CloudSaveServiceProvider : BaseService
    {
        public async Task<string> GetOne(string key)
        {
            var keySet = new HashSet<string> { key };
            var savedData = await CloudSaveService.Instance.Data.LoadAsync(keySet);
            if (!savedData.ContainsKey(key)) return null;
            return savedData[key];
        }

        public async Task<T> GetOne<T>(string key)
        {
            var res = await GetOne(key);
            if(res == null) return default;
            var asT = JsonUtility.FromJson<T>(res);
            return asT;
        }

        public async Task SaveOne(Dictionary<string, object> data)
        {
            Debug.Log($"Saving network data with key {data.Keys}");
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }

        public IEnumerator SaveOneRoutine(Dictionary<string, object> data)
        {
            yield return StartCoroutine(SaveOne(data).ToRoutine());
        }
    }
}