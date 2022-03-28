using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BraidsAccounting.DAL.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<StoreItem> Store { get; set; } = null!;
        public DbSet<WastedItem> WastedItems { get; set; } = null!;
        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;


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

    /// <summary>
    /// Фабрика создания контекста для Dependency Injection.
    /// </summary>
    public class MyServiceFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public static ApplicationContext CreateDbContext() =>
            new ApplicationContext(
                new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(ApplicationContext.GetConnectionString()).Options);

        /// <summary>
        /// Создание экземпляра ApplicationContext.
        /// </summary>
        public ApplicationContext CreateDbContext(string[] args) => CreateDbContext();
    }
}
