using UnityEngine;

namespace CofyEngine
{
    //TODO: Add custom network simulation 
    public class NetworkReachability
    {
        public static bool reachable => Application.internetReachability == UnityEngine.NetworkReachability.NotReachable;
    }
    
    public enum NetworkSimulate: byte
    {
        None,
        NoNetwork,
        Reachable,
    };
}