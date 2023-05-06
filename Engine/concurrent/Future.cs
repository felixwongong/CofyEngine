using System;

public abstract class Future<T>
{
}

public class Success<T> : Future<T>
{
    public T result;
    
    public Success(T result)
    {
        this.result = result;
    }
}

public class Failure<T> : Future<T>
{
    public Exception ex;

    public Failure(Exception ex)
    {
        this.ex = ex;
    }
}
