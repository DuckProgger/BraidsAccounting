using BraidsAccounting.DAL.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BraidsAccounting.Views.Converters;

internal class DateConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime date = (DateTime)value;
        return date.ToLongDateString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 
        throw new NotImplementedException();
}
