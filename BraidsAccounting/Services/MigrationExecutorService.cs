using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace BraidsAccounting.Services
{
    internal class MigrationExecutorService : IMigrationExecutorService
    {
        private readonly ApplicationContext context;
        private readonly IRepository<DatabaseVersion> versions;
        private const string configPath = "appsettings.json";
        private const string databaseVersionNode = "DatabaseVersion";


        public MigrationExecutorService(ApplicationContext context, IRepository<DatabaseVersion> versions)
        {
            this.context = context;
            this.versions = versions;
        }

        public void UpdateDatabase()
        {
            // Получить актуальную версию базы данных
            int actualVersion = versions.Items.Max(v => v.Version);

            // Сравнить её с последней версией
            var node = JObject.Parse(File.ReadAllText(configPath))[databaseVersionNode];
            if (node is null) return;
            int lastVersion = node.Value<int>();
            if (actualVersion >= lastVersion) return;

            // Обновить БД, если требуется
            for (int newVersion = actualVersion + 1; newVersion <= lastVersion; newVersion++, actualVersion++)
                Execute(newVersion);

            // Обновить актуальную версию БД
            versions.Create(new DatabaseVersion() { Version = actualVersion });
        }


        private void Execute(int version)
        {
            try
            {
                string migrationPath = @$"{Environment.CurrentDirectory}\Migrations\Migration {version}.sql";
                string queryString = File.ReadAllText(migrationPath);
                context.Database.ExecuteSqlRaw(queryString);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
