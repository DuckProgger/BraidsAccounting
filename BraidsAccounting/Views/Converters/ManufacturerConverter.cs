using BraidsAccounting.DAL.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BraidsAccounting.Views.Converters;

internal class ManufacturerConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Manufacturer) return null;
        return ((Manufacturer)value)?.Name ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
