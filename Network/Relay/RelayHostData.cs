using System;

namespace CM.Network.RelayUtil
{
    public struct RelayHostData
    {
        public string IPv4Addr;
        public ushort port;
        public Guid allocId;
        public byte[] allocIdBytes;
        public byte[] connData;
        public byte[] HmacKey;
        public string joinCode;
    }
}