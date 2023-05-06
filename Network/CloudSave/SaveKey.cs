using JetBrains.Annotations;
using UnityEngine;

namespace CM.Network.CloudSave
{
    public struct SaveKey
    {
        public static readonly string USER_COL = "user";
        public static readonly string USER_REF = "user-ref";
        public static readonly string PLAYER_ENTITY = "player";
        public static readonly string ACTIVE_MONSTERS = "monster";
        public static readonly string BAG_MONSTERS = "monsters-in-bag";
        public static readonly string TOKEN = "login-session-token";
        public static readonly string INVENTORY = "inventory";
        public static readonly string TEAMS = "teams";
        public static readonly string CLIENT_ID = "client-id";
    }
}