using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Infrastructure;

internal class Formatter
{
    //public string GetMoneyStringFormat(decimal amount) =>
    //     $"{amount:#.##} {CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol}";

    //public string GetMoneyStringFormat() =>
    //     $"{0:#.##} {CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol}";

    public static string GetMoneyStringFormat() =>
        "{0:#.##} ₽";
}
