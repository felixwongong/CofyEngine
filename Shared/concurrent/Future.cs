using System;

public interface IFuture
{
    public Exception ex { get; set; }
    
    public float progress { get; }
    
    public bool hasException => ex != null;
    
    public bool isCompleted { get; }
    public bool isSucceed { get; }
    public bool isFailure { get; }
}

public partial class Future<T>: IFuture
{
    public T result { get; set; }
    public Exception ex { get; set; }
    
    public float progress => _promise.progressFunc?.Invoke() ?? 0;
    
    public bool hasException => ex != null;
    public bool isCompleted => _promise.isCompleted;
    public bool isSucceed => _promise.isSucceed;
    public bool isFailure => _promise.isFailure;

    private readonly Promise<T> _promise;

    public Future() { }

    public Future(Promise<T> promise): this()
    {
        this._promise = promise;
    }
    
    public void OnCompleted(Action<Validation<T>> action) { _promise.OnCompleted(action); }

    public void OnSucceed(Action<T> action) { _promise.OnSucceed(action); }

    public void OnFailed(Action<Future<T>> action) { _promise.OnFailed(action); }
}

public partial class Future<T>
{
    public static Future<T> failure(Exception ex)
    {
        var failure = new Future<T>
        {
            ex = ex
        };
        return failure;
    }

    public Future<T> Then(Action<Future<T>> action)
    {
        Promise<T> promise = new Promise<T>(this._promise.progressFunc);
        
        this._promise.OnCompleted(validation =>
        {
            var future = validation.target;
            if (validation.hasException)
            {
                FLog.LogException(future.ex);
                promise.Reject(future.ex);
            }
            else
            {
                action(future);
                promise.Resolve(future.result);
            }
        });
        
        return promise.future;
    }

    public Future<A> TryMap<A>(Func<T, A> mapFunc)
    {
        Promise<A> promise = new Promise<A>(this._promise.progressFunc);

        this._promise.OnCompleted(validation =>
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
        });
        
        return promise.future;
    }
}

public sealed class Validation<T> 
{
    public bool hasException;
    public Future<T> target;

    public Validation(Future<T> future)
    {
        hasException = future.hasException;
        target = future;
    }
}
