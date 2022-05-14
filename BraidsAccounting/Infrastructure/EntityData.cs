using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BraidsAccounting.Infrastructure;

public class EntityData : IEnumerable<KeyValuePair<string, object>>, IEquatable<EntityData>
{
    private readonly Dictionary<string, object> propertiesInfo = new();

    public object this[string name] => propertiesInfo[name];
    public KeyValuePair<string, object> this[int idx] => propertiesInfo.ElementAt(idx);

    public EntityData(string entityName)
    {
        EntityName = entityName;
    }

    public string EntityName { get; } = null!;
    public int Count => propertiesInfo.Count;
    public void Add(string propertyName, object value) =>
        propertiesInfo.Add(propertyName, value);

    public void Remove(string propertyName) =>
        propertiesInfo.Remove(propertyName);

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        foreach (var propertyInfo in propertiesInfo)
            yield return propertyInfo;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Equals(EntityData? other)
    {
        foreach (var thisPropretyData in this)
        {
            var otherPropertyValue = other?[thisPropretyData.Key];
            if (otherPropertyValue == null) return false;
            if (!thisPropretyData.Value.Equals(otherPropertyValue)) return false;
        }
        return true;
    }

    public override bool Equals(object? obj) =>
         Equals(obj as EntityData);

}


