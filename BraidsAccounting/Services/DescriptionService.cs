using System;
using System.ComponentModel;

namespace BraidsAccounting.Services;

public static class DescriptionService
{
    public static string Get<T>(string propertyName)
    {
        AttributeCollection attributes = TypeDescriptor.GetProperties(typeof(T))[propertyName].Attributes;
        return GetDescriptionAttribute(attributes).Description;
    }

    public static string Get<T>()
    {
        AttributeCollection attributes = TypeDescriptor.GetAttributes(typeof(T));
        return GetDescriptionAttribute(attributes).Description;
    }

    public static string Get(Type propertyType, string propertyName)
    {
        AttributeCollection attributes = TypeDescriptor.GetProperties(propertyType)[propertyName].Attributes;
        return GetDescriptionAttribute(attributes).Description;
    }

    public static string Get(Type entityType)
    {
        AttributeCollection attributes = TypeDescriptor.GetAttributes(entityType);
        return GetDescriptionAttribute(attributes).Description;
    }

    private static DescriptionAttribute GetDescriptionAttribute(AttributeCollection attributeCollection) =>
        (DescriptionAttribute)attributeCollection[typeof(DescriptionAttribute)];
}
