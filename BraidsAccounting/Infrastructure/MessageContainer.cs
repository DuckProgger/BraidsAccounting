namespace BraidsAccounting.Infrastructure
{
    internal class MessageContainer
    {
        public static string Get(string key) =>
            (string)App.Current.Resources[key];
    }
}
