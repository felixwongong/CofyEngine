using System;

public class Future<T>
{
    public T result { get; set; }
    public Exception ex { get; set; }

    public readonly Promise<T> promise;

    public bool hasException => ex != null;
    
    public Future() { }

    public Future(Promise<T> promise): this()
    {
        this.promise = promise;
    }

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
        Promise<T> promise = new Promise<T>(this.promise.progressFunc);
        this.promise.Completed += validation =>
        {
            var future = validation.target;
            action(future);
            if (validation.hasException)
                promise.Reject(future.ex);
            else
                promise.Resolve(future.result);
        };
        return promise.future;
    }

    public Future<A> TryMap<A>(Func<T, A> mapFunc)
    {
        Promise<A> promise = new Promise<A>(this.promise.progressFunc);
        this.promise.Completed += validation =>
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
