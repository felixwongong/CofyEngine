using cofydev.util;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ToPromiseHandler
{
    public static PromiseImpl<T> ToPromise<T>(this AsyncOperationHandle<T> op)
    {
        PromiseImpl<T> promise = new PromiseImpl<T>(() => op.PercentComplete);
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