using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Dumb interface for generic typeless Promise 
/// </summary>
public interface IPromise
{
    public bool isCompleted { get; set; }
    public bool isSucceed { get; set; }
    public bool isFailure { get; set; }
    public Func<float> progressFunc { get; set; }
}

public class Promise<T>: IPromise
{
    public bool isCompleted { get; set; }
    public bool isSucceed { get; set; }
    public bool isFailure { get; set; }
    public Func<float> progressFunc { get; set; }

    private Future<T> _future = null;

    public Future<T> future
    {
        get => _future ??= new Future<T>(this);
    }

    public event Action<Validation<T>> Completed;
    public event Action<T> Succeed;
    public event Action<Future<T>> Failed;

    public Promise()
    {
        isCompleted = isSucceed = isFailure = false;
        progressFunc = () => 0;
    }

    public Promise(Func<float> progressFunc): this()
    {
        this.progressFunc = progressFunc;
    }

    public void Resolve(T result)
    {
        if (Mathf.Approximately(progressFunc(), 0)) progressFunc = () => 1;
        isCompleted = true;
        isSucceed = true;
        future.result = result;
        Completed?.Invoke(new Validation<T>(_future));
        Succeed?.Invoke(result);
        clear();
    }

    public void Reject(string msg)
    {
        Exception ex = new Exception(msg);
        Reject(ex);
    }

    public void Reject(Exception ex)
    {
        isCompleted = true;
        isFailure = true;
        future.ex = ex;
        Completed?.Invoke(new Validation<T>(_future));
        Failed?.Invoke(_future);
        clear();
    }

    private void clear()
    {
        Completed = null;
        Succeed = null;
        Failed = null;
    }

}
