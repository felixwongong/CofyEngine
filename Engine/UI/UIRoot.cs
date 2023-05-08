using CM.Util.Singleton;
using UnityEngine;

namespace CofyEngine
{
    public class UIRoot : SingleBehaviour<UIRoot>
    {
        public Promise<GameObject> Bind<T>(Promise<GameObject> uiInstantiation) where T: UIInstance<T>
        {
            return uiInstantiation.TryMap(go => go.GetComponent<T>())
                .Then(future =>
                {
                    UIInstance<T>.instance = future.result;
                    future.result.transform.SetParent(transform);
                })
                .TryMap(t => t.gameObject);
        }
    }
}