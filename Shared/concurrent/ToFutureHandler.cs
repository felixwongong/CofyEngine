using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ToFutureHandler
{
    public static Future<AsyncOperationHandle<T>> Future<T>(this AsyncOperationHandle<T> op)
    {
        Promise<AsyncOperationHandle<T>> promise = new Promise<AsyncOperationHandle<T>>(() => op.PercentComplete);
        op.Completed += handle =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    promise.Resolve(handle);
                    break;
                case AsyncOperationStatus.Failed:
                    promise.Reject(op.OperationException);
                    break;
            }
        };

        return promise.future;
    }
    
    public static Future<AsyncOperation> Future(this AsyncOperation op)
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

        return promise.future;
    }

    public static Future<ResourceRequest> Future(this ResourceRequest req)
    {
        Promise<ResourceRequest> promise = new();
        req.completed += aop =>
        {
            if (Mathf.Approximately(req.progress, 1))
            {
                promise.Resolve(req);
            }
            else
            {
                promise.Reject("Async operation failed");
            }
        };
        return promise.future;
    }

    private static IEnumerator ToCoroutine(this AsyncOperation op)
    {
        yield return op;
    }

    public static Future<bool> Future(this Task task)
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
        return promise.future;
    }
}