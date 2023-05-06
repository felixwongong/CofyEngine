using Unity.Collections;
using Unity.Netcode;

namespace CM.Network
{
    public struct NetworkString : INetworkSerializeByMemcpy
    {
        private ForceNetworkSerializeByMemcpy<FixedString32Bytes> _info;
        public FixedString32Bytes info => _info.Value;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _info);
        }

        public override string ToString()
        {
            return _info.Value.ToString();
        }
        

        public static implicit operator string(NetworkString s)
        {
            return s.ToString();
        }

        public static implicit operator NetworkString(string s)
        {
            return new NetworkString { _info = new FixedString32Bytes(s) };
        }
    }
}