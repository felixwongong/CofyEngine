using System;

public class Future<T>
{
    public T result { get; set; }
    public Exception ex { get; set; }

    public bool hasException => ex != null;
    
    public Future() { }

    public static Future<T> failure(Exception ex)
    {
        var failure = new Future<T>
        {
            ex = ex
        };
        return failure;
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
