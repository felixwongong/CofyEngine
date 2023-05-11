using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dumb interface for generic typeless Promise 
/// </summary>
public interface IPromise
{
    public bool isDone { get; set; }
    public Func<float> progressFunc { get; set; }
}

public class Promise<T>: IPromise
{
    public bool isDone { get; set; }
    public Func<float> progressFunc { get; set; }
    
    public event Action<Validation<T>> Completed;
    public event Action<T> Succeed;
    public event Action<Failure<T>> Failed;

    public Promise(Func<float> progressFunc)
    {
        this.progressFunc = progressFunc;
    }

    public void Resolve(T result)
    {
        isDone = true;
        Completed?.Invoke(new Validation<T>(new Success<T>(result)));
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
        isDone = true;
        Completed?.Invoke(new Validation<T>(new Failure<T>(ex)));
        Failed?.Invoke(new Failure<T>(ex));
        clear();
    }

    public Promise<T> Then(Action<Future<T>> action)
    {
        Promise<T> promise = new Promise<T>(this.progressFunc);
        this.Completed += future =>
        {
            if (future.hasException)
            {
                var failure = future.target as Failure<T>;
                action(failure);
                promise.Reject(failure.ex);
            }
            else
            {
                var success = future.target as Success<T>;
                action(success);
                promise.Resolve(success.result);
            }
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
                var failure = validation.target as Failure<T>;
                promise.Reject(failure.ex);
            }
            else
            {
                var success = validation.target as Success<T>;
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
