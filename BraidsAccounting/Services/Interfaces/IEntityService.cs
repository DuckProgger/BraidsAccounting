using BraidsAccounting.DAL.Entities.Base;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IEntityService<T> : IHistoryTracer<T> where T : IEntity
    {
        bool Validate(T entity, out IEnumerable<string> errorMessages);
    }
}
