using System;
using System.Collections;
using System.Collections.Generic;

namespace BraidsAccounting.Infrastructure;

public class EntityData : IEnumerable<PropertyData> 
{
    private readonly List<PropertyData> propertiesInfo = new();

    public PropertyData this[int x] => propertiesInfo[x];
    public EntityData(string entityName)
    {
        EntityName = entityName;
    }

    public string EntityName { get; } = null!;
    public int Count => propertiesInfo.Count;
    public void Add(string propertyName, object value)
    {
        PropertyData propertyInfo = new()
        {
            Name = propertyName,
            Value = value
        };
        propertiesInfo.Add(propertyInfo);
    }

    public IEnumerator<PropertyData> GetEnumerator()
    {
        foreach (var propertyInfo in propertiesInfo)
            yield return propertyInfo;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}


