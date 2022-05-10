using BraidsAccounting.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BraidsAccounting.Infrastructure;
internal class EntityDataBuilder : IEntityDataBuilder
{
    private Type? entityType;
    private EntityData propertyDatas = null!;

    public IEntityDataBuilder AddInfo<T>(Expression<Func<T>> expression, object value)
    {
        Expression propertyExpression = expression.Body;

        // Найти имя корневой сущности
        if (entityType is null)
        {
            entityType = GetEntityType(propertyExpression);
            string entityLocalizedName = DescriptionService.Get(entityType);
            propertyDatas = new(entityLocalizedName);
        }

        // Получить имя свойства
        MemberExpression memberExpression = (MemberExpression)propertyExpression;
        string propertyName = GetPropertyName(memberExpression);

        // Получить тип сущности, в которой находится свойство
        memberExpression = (MemberExpression)memberExpression.Expression;
        Type propertyOwnerType = GetPropertyOwnerType(memberExpression);

        // Получить локализованное имя свойства
        string propertyLocalizedName = propertyOwnerType.Equals(entityType)
          ? DescriptionService.Get(entityType, propertyName)
          : $"{DescriptionService.Get(propertyOwnerType)}_{DescriptionService.Get(propertyOwnerType, propertyName)}";

        // Заполнить данные о свойстве
        propertyDatas.Add(propertyLocalizedName, value);

        return this;
    }

    public EntityData GetPropertyDatas() => propertyDatas;

    private static Type GetEntityType(Expression propertyExpression)
    {
        Stack<Type> chainTypes = new();
        while (propertyExpression is MemberExpression temp)
        {
            propertyExpression = temp.Expression;
            if (temp.Member is PropertyInfo propertyInfo)
            {
                chainTypes.Push(propertyInfo.PropertyType);
                continue;
            }
            if (temp.Member is FieldInfo fieldInfo)
            {
                chainTypes.Push(fieldInfo.FieldType);
                continue;
            }
        }
        return chainTypes.Pop();
    }

    private static string GetPropertyName(MemberExpression propertyExpression) =>
        (propertyExpression.Member as PropertyInfo).Name;

    private static Type GetPropertyOwnerType(MemberExpression propertyExpression)
    {
        if (propertyExpression.Member is PropertyInfo propertyInfo)
            return propertyInfo.PropertyType;
        if (propertyExpression.Member is FieldInfo fieldInfo)
            return fieldInfo.FieldType;
        throw new ArgumentException("Недопустимый член");
    }

}

