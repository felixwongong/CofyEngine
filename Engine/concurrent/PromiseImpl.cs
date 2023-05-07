using System;

/// <summary>
/// Dumb interface for generic typeless Promise 
/// </summary>
public interface Promise
{

}

public class PromiseImpl<T>: Promise
{
    public bool isDone = false;
    public Func<float> progresFunc;

    public event Action<Validation<T>> Completed;
    public event Action<T> Succeed;
    public event Action<Failure<T>> Failed;

    public PromiseImpl(Func<float> progressFunc)
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

    public PromiseImpl<T> Then(Action<Future<T>> action)
    {
        PromiseImpl<T> promise = new PromiseImpl<T>(this.progresFunc);
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

    public PromiseImpl<A> TryMap<A>(Func<T, A> mapFunc)
    {
        PromiseImpl<A> promise = new PromiseImpl<A>(this.progresFunc);
        this.Completed += future =>
        {
            if (future.hasException)
            {
                var failure = future.target as Failure<T>;
                promise.Reject(failure.ex);
            }
            else
            {
                var success = future.target as Success<T>;
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
