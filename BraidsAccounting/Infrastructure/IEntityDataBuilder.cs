using BraidsAccounting.DAL.Entities.Base;
using System;
using System.Linq.Expressions;

namespace BraidsAccounting.Infrastructure;

internal interface IEntityDataBuilder<TEntity> where TEntity : IEntity
{
    IEntityDataBuilder<TEntity> AddInfo<TProperty>(Expression<Func<TEntity, TProperty>> expression, TProperty value);
    EntityData GetEntityData();
}

