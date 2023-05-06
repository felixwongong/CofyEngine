using System;
using cofydev.util;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Promise<T>
{
    public bool isDone = false;
    public Func<float> progresFunc;

    public event Action<Validation<T>> Completed;
    public event Action<T> Succeed;
    public event Action<Failure<T>> Failed;

    public Promise(Func<float> progressFunc)
    {
        this.progresFunc = progressFunc;
    }

    public void Resolve(T result)
    {
        Completed?.Invoke(new Validation<T>(new Success<T>(result)));
        Succeed?.Invoke(result);
        isDone = true;
        clear();
    }

    public void Reject(string msg)
    {
        Exception ex = new Exception(msg);
        Reject(ex);
    }

    public void Reject(Exception ex)
    {
        Completed?.Invoke(new Validation<T>(new Failure<T>(ex)));
        Failed?.Invoke(new Failure<T>(ex));
        isDone = true;
        clear();
    }

    public Promise<T> Then(Action<Future<T>> action)
    {
        Promise<T> promise = new Promise<T>(this.progresFunc);
        this.Completed += future =>
        {
            action(future);
            if (future.hasException)
            {
                promise.Reject(future.ex);
            }
            else
            {
                promise.Resolve(future.result);
            }
        };
        return promise;
    }

    private void clear()
    {
        Completed = null;
        Succeed = null;
        Failed = null;
    }
}
