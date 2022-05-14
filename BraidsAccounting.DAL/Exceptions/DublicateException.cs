namespace BraidsAccounting.DAL.Exceptions;

public class DublicateException : Exception
{
    public DublicateException(string? message) : base(message)
    {
    }
}
