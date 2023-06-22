using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CofyEngine.Engine.Util;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Logger = UnityEngine.Logger;
using Random = UnityEngine.Random;

namespace CM.Network.LobbyUtil
{
    // NEED TO ADD EVENT SUBSCRIPTION ONCE UNITY LOBBY SUPPORT
    public class LobbyServiceProvider : BaseService
    {
        //CONFIG
        [SerializeField] private string lobbyNameBase = "Lobby";

        [Range(2, 8)] [SerializeField] private int defaultMaxPlayer = 4;

        [SerializeField] private float heartBeatWaitTime = 15f;
        public Lobby curLobby;
        private string curLobbyId;

        //STATE
        private ConcurrentQueue<string> existingLobbyIds;
        private string hostingLobbyId;

        //TODO: Add action queue to improve code logic in the future
        private int lastPollTime;
        private int lastUpdateTime;

        private async void OnApplicationQuit()
        {
            await DeleteOrLeaveLobbies();
        }

        #region Create Lobby

        private async Task<Lobby> CreateLobby(int maxPlayer = -1, string lobbyName = null,
            CreateLobbyOptions options = null)
        {
            lobbyName = string.IsNullOrEmpty(lobbyName) ? lobbyNameBase + Random.Range(0, 100) : lobbyName;
            maxPlayer = maxPlayer == -1 ? defaultMaxPlayer : maxPlayer;

            options ??= new CreateLobbyOptions
            {
                IsPrivate = false
            };

            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, options);

            hostingLobbyId = lobby.Id;
            curLobbyId = lobby.Id;
            return lobby;
        }

        public async Task<Lobby> CreateLobbyWithHeartbeat(int maxPlayer = 2, string lobbyName = null,
            CreateLobbyOptions options = null)
        {
            var lobby = await CreateLobby(maxPlayer, lobbyName, options);

            StartCoroutine(HeartbeatLobby());
            return lobby;
        }

        private IEnumerator HeartbeatLobby()
        {
            while (!string.IsNullOrEmpty(curLobbyId))
            {
                LobbyService.Instance.SendHeartbeatPingAsync(curLobbyId);
                yield return new WaitForSecondsRealtime(heartBeatWaitTime);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        #endregion

        #region Lobby Operation

        public async Task<Lobby> JoinLobby(string lobbyId = null, string lobbyCode = null, Player player = null)
        {
            if (player == null)
                UpdatePlayerWithData(ref player, "player-name", $"Unknown Game PP -- {Random.Range(0, 10)}");

            if (string.IsNullOrEmpty(lobbyId) && string.IsNullOrEmpty(lobbyCode)) return await QuickJoin(player);

            if (lobbyId != null)
            {
                var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, new JoinLobbyByIdOptions
                {
                    Player = player
                });
                curLobbyId = lobby.Id;
                return lobby;
            }

            return null;
        }

        private async Task<Lobby> QuickJoin(Player player = null)
        {
            var opts = new QuickJoinLobbyOptions
            {
                Player = player
            };

            var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(opts);
            curLobbyId = lobby.Id;
            return lobby;
        }

        public async Task<List<Lobby>> QueryLobbies()
        {
            var opt = new LobbyQueryFactory(2)
                .Greater(QueryFilter.FieldOptions.AvailableSlots, 0, false).GetQuery();
            opt.Count = 1;
            var lobbies = await Lobbies.Instance.QueryLobbiesAsync(opt);
            
            return lobbies.Results.Where(lobby => !lobby.IsLocked && !lobby.IsPrivate).ToList();
        }

        public async Task DeleteOrLeaveLobbies()
        {
            if (!AuthenticationService.Instance.IsSignedIn) return;
            if (string.IsNullOrEmpty(hostingLobbyId))
            {
                if (curLobbyId != null) await LeaveLobby();
                return;
            }

            await GetCurLobby();

            if (curLobby.Players.Count <= 1)
            {
                Debug.Log($"Deleting Lobby with ID {curLobbyId}");
                await LobbyService.Instance.DeleteLobbyAsync(curLobby.Id);
            }
            else
            {
                await LeaveLobby();
            }

            hostingLobbyId = "";
            curLobbyId = "";
            StopAllCoroutines();
        }

        public async Task LeaveLobby()
        {
            if (string.IsNullOrEmpty(curLobbyId)) return;
            try
            {
                Debug.Log($"Leaving Lobby with ID {curLobbyId}");
                var playerId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(curLobbyId, playerId);
                curLobbyId = "";
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        #endregion

        #region Util

        public void UpdatePlayerWithData(ref Player playerRef, string key, string value,
            PlayerDataObject.VisibilityOptions visibility = PlayerDataObject.VisibilityOptions.Member)
        {
            playerRef ??= new Player();
            playerRef.Data ??= new Dictionary<string, PlayerDataObject>();

            if (!playerRef.Data.ContainsKey(key))
                playerRef.Data.Add(key, new PlayerDataObject(visibility, value));
            else
                playerRef.Data[key] = new PlayerDataObject(visibility, value);
        }

        public async Task UpdateLobbyWithData(string key, string value, Lobby lobby = null,
            DataObject.VisibilityOptions visibility = DataObject.VisibilityOptions.Member)
        {
            await UpdateLobbyWithData(new Dictionary<string, DataObject>
                {
                    { key, new DataObject(visibility, value) }
                }, lobby, visibility);
        }

        public async Task UpdateLobbyWithData(Dictionary<string, DataObject> keypair, Lobby lobby = null,
            DataObject.VisibilityOptions visibility = DataObject.VisibilityOptions.Member)
        {
            var timeSinceLastUpdate = Math.Abs(DateTime.Now.Millisecond - lastUpdateTime);
            Debug.Log($"{timeSinceLastUpdate} pass since last lobby update");
            if (timeSinceLastUpdate <= 1000)
            {
                await Task.Delay(1000 - timeSinceLastUpdate);
            }

            lastUpdateTime = DateTime.Now.Millisecond;
            
            lobby ??= await GetCurLobby();
            if(lobby == null) return;

            var options = new UpdateLobbyOptions
            {
                HostId = lobby.HostId,
                IsLocked = lobby.IsLocked,
                IsPrivate = lobby.IsPrivate,
                MaxPlayers = lobby.MaxPlayers,
                Name = lobby.Name
            };

            if (lobby.Data == null)
            {
                options.Data = keypair;
            }
            else
            {
                options.Data = lobby.Data;
                foreach (var (k, v) in keypair)
                {
                    if (options.Data.ContainsKey(k))
                    {
                        options.Data[k] = v;
                    }
                    else
                    {
                        options.Data.Add(k, v);
                    }
                }
            }

            await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, options);
        }

        public async Task<bool> hasLobby()
        {
            var lobbies = await QueryLobbies();
            print($"Has {lobbies.Count} lobbies");
            return lobbies.Count != 0;
        }


        public async Task<List<string>> GetPublicLobby()
        {
            return await LobbyService.Instance.GetJoinedLobbiesAsync();
        }

        private bool isWaitPolling = false;
        public async Task<Lobby> GetCurLobby(EPollMode pollMode = EPollMode.QUEUE)
        {
            if (pollMode == EPollMode.QUEUE)
            {
                var timeSinceLastPoll = Math.Abs(DateTime.Now.Millisecond - lastPollTime);
                Debug.Log($"{timeSinceLastPoll} pass since last lobby polling");
                if (timeSinceLastPoll <= 1000)
                {
                    if (!isWaitPolling)
                    {
                        await Task.Delay(1100);
                    }
                    else
                    {
                        await Task.Delay(1100);
                        return curLobby;
                    }
                }
            }

            try
            {
                if (string.IsNullOrEmpty(curLobbyId)) return null;
                curLobby = await LobbyService.Instance.GetLobbyAsync(curLobbyId);
                CheckHost(curLobby);
                lastPollTime = DateTime.Now.Millisecond;
                isWaitPolling = false;
                return curLobby;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return null;
        }

        private void CheckHost(Lobby lobby)
        {
            if (lobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                Debug.Log("I am the host");
                hostingLobbyId = lobby.Id;
            }
            else
            {
                hostingLobbyId = "";
            }
        }

        public bool isHost()
        {
            return !string.IsNullOrEmpty(hostingLobbyId);
        }

        #endregion
    }

    public enum EPollMode
    {
        IMMEDIATE,
        QUEUE
    }
}