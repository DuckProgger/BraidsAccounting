using BraidsAccounting.DAL.Entities.Base;
using BraidsAccounting.Services.Interfaces;

namespace BraidsAccounting.Infrastructure
{
    internal static class EntityDataExtensions
    {
        public static EntityData GetEtityData<TEntity>(this TEntity entity, IHistoryTracer<TEntity> tracer) where TEntity : IEntity
        {
            EntityDataBuilder<TEntity> builder = new();
            tracer.ConfigureEntityData(builder, entity);
            return builder.GetEntityData();
        }
    }
}
