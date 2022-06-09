using BraidsAccounting.DAL.Entities.Base;
using BraidsAccounting.Services;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BraidsAccounting.Infrastructure;
internal class EntityDataBuilder<TEntity> : IEntityDataBuilder<TEntity> where TEntity : IEntity
{
    private readonly EntityData entityData;

    public EntityDataBuilder()
    {
        string entityLocalizedName = DescriptionService.Get<TEntity>();
        entityData = new(entityLocalizedName);
    }

    public IEntityDataBuilder<TEntity> AddInfo<TProperty>(Expression<Func<TEntity, TProperty>> expression, TProperty value)
    {
        if (value is null) return this;
        Expression propertyExpression = expression.Body;
        MemberExpression memberExpression = (MemberExpression)propertyExpression;

        // Получить имя свойства
        string propertyName = ((PropertyInfo)memberExpression.Member).Name;

        // Получить тип сущности, в которой находится свойство       
        Type propertyOwnerType = memberExpression.Expression.Type;

        // Получить локализованное имя свойства
        string propertyLocalizedName = propertyOwnerType.Equals(typeof(TEntity))
          ? DescriptionService.Get<TEntity>(propertyName)
          : $"{DescriptionService.Get(propertyOwnerType)}_{DescriptionService.Get(propertyOwnerType, propertyName)}";

        // Заполнить данные о свойстве
        entityData.Add(propertyLocalizedName, value);

        return this;
    }

    public EntityData GetEntityData() => entityData;

    public static EntityData GetEntityData(Func<IEntityDataBuilder<TEntity>, IEntityDataBuilder<TEntity>> configuration)
    {
        var builder = new EntityDataBuilder<TEntity>();
        configuration.Invoke(builder);
        return builder.GetEntityData();
    }
}

