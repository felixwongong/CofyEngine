﻿using System;
using System.Collections.Generic;
using CofyEngine;
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
    private Validation<T> result { get; set; }

    private Future<T> _future = null;

    public Future<T> future
    {
        get => _future ??= new Future<T>(this);
    }

    private event Action<Validation<T>> Completed;
    private event Action<T> Succeed;
    private event Action<Future<T>> Failed;
    
    public Promise()
    {
        Clear();
    }

    public Promise(Func<float> progressFunc): this()
    {
        this.progressFunc = progressFunc;
    }

    public void Resolve(T result)
    {
        if (progressFunc == null || Mathf.Approximately(progressFunc(), 0)) progressFunc = () => 1;
        isCompleted = true;
        isSucceed = true;
        future.result = result;
        this.result = new Validation<T>(_future);
        Completed?.Invoke(this.result);
        Succeed?.Invoke(result);
        Clear();
    }

    public void ResolveFrom(Future<T> future)
    {
        if (future.isSucceed)
        {
            var progress = future.progress;
            this.progressFunc = () => progress;
            Resolve(future.result);
        } else if (future.isFailure && future.hasException)
        {
            Reject(future.ex);
        }
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
        this.result = new Validation<T>(_future);
        Completed?.Invoke(this.result);
        Failed?.Invoke(_future);
        Clear();
    }

    private void Clear()
    {
        Completed = null;
        Succeed = null;
        Failed = null;
    }

    public Promise<T> Reset(Func<float> progressFn = null)
    {
        isCompleted = isSucceed = isFailure = false;
        Clear();
        progressFunc = progressFn;
        result = null;
        _future = null;
        return this;
    }
    
    public void OnCompleted(Action<Validation<T>> action)
    {
        if (isCompleted) action(this.result);
        else Completed += action;
    }

    public void OnSucceed(Action<T> action)
    {
        if (isCompleted)
        {
            if (isSucceed)
                action(this.result.result);
            else
                FLog.LogWarning("Registered OnSucceed after promise failed.");
        }
        else Succeed += action;
    }

    public void OnFailed(Action<Future<T>> action)
    {
        if (isCompleted)
        {
            if (isFailure)
                action(this.result.target);
            else
                FLog.LogWarning("Registered OnFailed after promise succeed.");
        }
        else Failed += action;
    }
}
