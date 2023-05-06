using System;

namespace CM.Network.RelayUtil
{
    public struct RelayJoinData
    {
        public string joinCode;
        public string IPv4Addr;
        public ushort port;
        public Guid allocId;
        public byte[] allocIdBytes;
        public byte[] connData;
        public byte[] key;
        public byte[] hostConnData;
    }
}