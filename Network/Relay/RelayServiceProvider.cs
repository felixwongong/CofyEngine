using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using UnityEngine;

namespace CM.Network.RelayUtil
{
    public class RelayServiceProvider : BaseService
    {
        [SerializeField] private string envir = "DEVELOPMENT";
        [SerializeField] [Range(2, 10)] private int numConnections = 2;

        private UnityTransport transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

        private bool isUsingRelay =>
            transport != null && transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;


        public async Task<RelayHostData> SetupHostRelay()
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName(envir);

            var allocation = await Relay.Instance.CreateAllocationAsync(numConnections);

            var relayServ = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");

            var relayHostData = new RelayHostData
            {
                allocId = allocation.AllocationId,
                port = (ushort)relayServ.Port,
                allocIdBytes = allocation.AllocationIdBytes,
                connData = allocation.ConnectionData,
                IPv4Addr = relayServ.Host,
                HmacKey = allocation.Key
            };

            relayHostData.joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);

            transport.SetRelayServerData(relayHostData.IPv4Addr, relayHostData.port, relayHostData.allocIdBytes,
                relayHostData.HmacKey, relayHostData.connData);

            NetworkManager.Singleton.GetComponent<UnityTransport>()
                .SetHostRelayData(relayHostData.IPv4Addr, relayHostData.port, relayHostData.allocIdBytes,
                    relayHostData.HmacKey, relayHostData.connData, true);
            NetworkManager.Singleton.StartHost();

            Debug.Log($"Relay Server started. Join Code: {relayHostData.joinCode}");
            return relayHostData;
        }

        public async Task<RelayJoinData> JoinRelay(string joinCode)
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var relayServ = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");

            var data = new RelayJoinData
            {
                allocId = allocation.AllocationId,
                port = (ushort)relayServ.Port,
                allocIdBytes = allocation.AllocationIdBytes,
                connData = allocation.ConnectionData,
                IPv4Addr = relayServ.Host,
                key = allocation.Key,
                hostConnData = allocation.HostConnectionData
            };


            NetworkManager.Singleton.GetComponent<UnityTransport>()
                .SetClientRelayData(data.IPv4Addr, data.port, data.allocIdBytes, data.key, data.connData,
                    data.hostConnData, true);
            NetworkManager.Singleton.StartClient();
            Debug.Log($"Relay Client started. Join Code: {joinCode}");
            return data;
        }
    }
}