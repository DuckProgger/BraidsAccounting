using BraidsAccounting.DAL.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BraidsAccounting.Views.Converters;

internal class EmployeeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Employee) return null;
        return ((Employee)value)?.Name ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 
        throw new NotImplementedException();
}
