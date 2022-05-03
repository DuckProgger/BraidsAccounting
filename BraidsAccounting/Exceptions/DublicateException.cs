using System;

namespace BraidsAccounting.Exceptions;

internal class DublicateException : Exception
{
    public DublicateException(string? message) : base(message)
    {
    }
}
