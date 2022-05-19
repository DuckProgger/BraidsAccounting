using BraidsAccounting.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace BraidsAccounting.DAL.Context;

public class ApplicationContext : DbContext
{
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<StoreItem> Store { get; set; } = null!;
    public DbSet<WastedItem> WastedItems { get; set; } = null!;
    public DbSet<Manufacturer> Manufacturers { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<History> History { get; set; } = null!;
    public DbSet<DatabaseVersion> DatabaseVersions { get; set; } = null!;


    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Service>(ServiceConfigure)
            .Entity<Payment>(PaymentConfigure)
            .Entity<History>(HistoryConfigure)
            .Entity<DatabaseVersion>(DatabaseVersionConfigure)
            ;
    }

    private void ServiceConfigure(EntityTypeBuilder<Service> builder)
    {       
        builder
          .Property(x => x.DateTime)
          .HasDefaultValueSql("GETDATE()");
    }

    private void PaymentConfigure(EntityTypeBuilder<Payment> builder)
    {
        builder
          .Property(x => x.DateTime)
          .HasDefaultValueSql("GETDATE()");
    }

    private void HistoryConfigure(EntityTypeBuilder<History> builder)
    {
        builder
          .Property(x => x.TimeStamp)
          .HasDefaultValueSql("GETDATE()");
    }

    private void DatabaseVersionConfigure(EntityTypeBuilder<DatabaseVersion> builder)
    {
        builder
          .Property(x => x.DateTime)
          .HasDefaultValueSql("GETDATE()");
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
