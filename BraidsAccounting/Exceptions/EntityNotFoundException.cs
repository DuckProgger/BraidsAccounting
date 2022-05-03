using System;

namespace BraidsAccounting.Exceptions;

internal class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string? message) : base(message)
    {
    }
}
