using System;
using System.Linq.Expressions;

namespace BraidsAccounting.Infrastructure;

internal interface IEntityDataBuilder
{
    IEntityDataBuilder AddInfo<T>(Expression<Func<T>> expression, object value);
    EntityData GetPropertyDatas();
}

