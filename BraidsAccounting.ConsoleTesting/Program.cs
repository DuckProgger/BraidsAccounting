using BraidsAccounting.DAL.Entities;

class Program
{
    static void Main()
    {
        Item item1 = new()
        {
            Article = "AbC",
            Color = "reD",
            Manufacturer = "company"
        };
        Item item2 = new()
        {
            Article = "Abc",
            Color = "ed",
            Manufacturer = "Company"
        };
        Console.WriteLine(item1.Equals(item2));
    }
}