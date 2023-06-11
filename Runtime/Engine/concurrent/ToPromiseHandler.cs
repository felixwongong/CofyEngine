using System.Threading.Tasks;
using CofyEngine.Runtime.Engine.Util;
using UnityEngine;
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
    
    public static Promise<AsyncOperation> ToPromise(this AsyncOperation op)
    {
        Promise<AsyncOperation> promise = new Promise<AsyncOperation>(() => op.progress);
        op.completed += aop =>
        {
            if (Mathf.Approximately(op.progress, 1))
            {
                promise.Resolve(aop);
            }
            else
            {
                promise.Reject("Async operation failed");
            }
        };
        
        return promise;
    }

    public static Promise<bool> ToPromise(this Task task)
    {
        Promise<bool> promise = new Promise<bool>(() =>
        {
            switch (task.Status)
            {
                case TaskStatus.Created:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.WaitingForChildrenToComplete:
                    return 0;

                case TaskStatus.Canceled: case TaskStatus.Faulted: case TaskStatus.Running:
                    return 0.5f;
                case TaskStatus.RanToCompletion:
                    return 1;
                default:
                    return -1;
            }
        });

        task.ContinueWith((t, o) =>
        {
            if (t.IsCompletedSuccessfully)
            {
                promise.Resolve(true);
            }
            else
            {
                promise.Reject(t.Exception);
            }
        }, TaskContinuationOptions.RunContinuationsAsynchronously);
        return promise;
    }
}