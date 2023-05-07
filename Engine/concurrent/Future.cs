using System;

public abstract class Future<T>
{
    public virtual T result { get; set; }
    public virtual Exception ex { get; set; }
}

public sealed class Success<T> : Future<T>
{
    public override T result { get; set; }
    
    public Success(T result)
    {
        this.result = result;
    }
}

public sealed class Failure<T> : Future<T>
{
    public override Exception ex { get; set; }

    public Failure(Exception ex)
    {
        this.ex = ex;
    }
}

public sealed class Validation<T> 
{
    public bool hasException;
    public Future<T> target;

    public Validation(Success<T> success)
    {
        hasException = false;
        target = success;
    }

    public Validation(Failure<T> failure)
    {
        hasException = false;
        target = failure;
    }
}
