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
        get => _future ??= new Future<T>();
    }

    public event Action<Validation<T>> Completed;
    public event Action<T> Succeed;
    public event Action<Future<T>> Failed;

    public Promise()
    {
        isCompleted = isSucceed = isFailure = false;
        progressFunc = null;
    }

    public Promise(Func<float> progressFunc)
    {
        this.progressFunc = progressFunc;
    }

    public void Resolve(T result)
    {
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

    public Promise<T> Then(Action<Future<T>> action)
    {
        Promise<T> promise = new Promise<T>(this.progressFunc);
        this.Completed += validation =>
        {
            var future = validation.target;
            action(future);
            if (validation.hasException)
                promise.Reject(future.ex);
            else
                promise.Resolve(future.result);
        };
        return promise;
    }

    public Promise<A> TryMap<A>(Func<T, A> mapFunc)
    {
        Promise<A> promise = new Promise<A>(this.progressFunc);
        this.Completed += validation =>
        {
            if (validation.hasException)
            {
                var failure = validation.target;
                promise.Reject(failure.ex);
            }
            else
            {
                var success = validation.target;
                A mappedValue = mapFunc(success.result);
                promise.Resolve(mappedValue);
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
