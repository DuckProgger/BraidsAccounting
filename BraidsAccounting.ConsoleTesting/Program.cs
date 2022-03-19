using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using BraidsAccounting.

class Program
{
    static void Main()
    {
        const string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Braids;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        using var db = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer(connection).Options);

        ServiceProvider serviceProvider = new()
    }
}