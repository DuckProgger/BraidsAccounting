using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace BraidsAccounting.Data
{
    internal static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services) => services
            .AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(GetConnectionString());
            })
            .AddRepositories()
            ;

        private static string GetConnectionString()
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
}
