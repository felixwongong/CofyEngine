using cofydev.util;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ToPromiseHandler
{
    public static Promise<T> ToPromise<T>(this AsyncOperationHandle<T> op)
    {
        Promise<T> promise = new Promise<T>(() => op.PercentComplete);
        op.Completed += handle =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    promise.Resolve(handle.Result);
                    break;
                case AsyncOperationStatus.Failed:
                    promise.Reject(op.OperationException);
                    break;
            }
        };
        
        return promise;
    }
}