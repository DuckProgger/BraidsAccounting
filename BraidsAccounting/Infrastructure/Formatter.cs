using BraidsAccounting.DAL.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BraidsAccounting.Infrastructure;

internal class Formatter
{
    public static string GetMoneyStringFormat() => "{0:0.##} ₽";

    public InlineCollection GetHistoryInlines(History history)
    {
        TextBlock textBlock = new();
        textBlock.Inlines.Add(new Run() { FontStyle = FontStyles.Italic });

        return textBlock.Inlines;

    }
}
