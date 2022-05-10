using BraidsAccounting.DAL.Entities.Base;
using BraidsAccounting.Infrastructure;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IHistoryTracer<TEntity> where TEntity : IEntity
    {
        IEntityDataBuilder<TEntity> ConfigureEntityData(IEntityDataBuilder<TEntity> builder, TEntity entity);
    }
}
