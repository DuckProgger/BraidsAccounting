using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.DAL.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<StoreItem> Store { get; set; }
        public DbSet<WastedItem> WastedItems { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(GetConnectionString());
        }

        internal static string GetConnectionString()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            IConfigurationRoot config = builder.Build();
            // получаем строку подключения
            return config.GetConnectionString("DefaultConnection");
        }

    }

    public class MyServiceFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public static ApplicationContext CreateDbContext()
        {
            return new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer(ApplicationContext.GetConnectionString()).Options);
        }

        public ApplicationContext CreateDbContext(string[] args) => CreateDbContext();
    }
}
