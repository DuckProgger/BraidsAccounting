using BraidsAccounting.Infrastructure;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BraidsAccounting.Views.Converters;

internal class PositiveMoneyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        decimal money = Math.Abs((decimal)value);
        return string.Format(Formatter.GetMoneyStringFormat(), money);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
