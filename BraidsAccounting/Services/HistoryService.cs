using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal class HistoryService : IHistoryService
    {
        private readonly IRepository<History> historyRepository;

        public HistoryService(IRepository<History> historyRepository)
        {
            this.historyRepository = historyRepository;
        }

        public async Task<List<History>> GetAllAsync() =>
           await historyRepository.Items.Take(50).OrderByDescending(h => h.TimeStamp).ToListAsync();

        public async Task WriteCreateOperationAsync(EntityData createdEntityData)
        {
            StringBuilder sb = new();

            // Тип операции:
            sb.Append("Добавлена сущность \"");

            // Получение названия типа сущности
            sb.Append(createdEntityData.EntityName);

            // Получение названий и значений свойств
            if (createdEntityData.Count > 0)
            {
                sb.Append("\". Значения: ");
                FillProperties(sb, createdEntityData);
            }
            sb.Append('.');

            await AddHistoryAsync(sb.ToString());
        }

        public async Task WriteDeleteOperationAsync(EntityData deletedEntityData)
        {
            StringBuilder sb = new();

            // Тип операции:
            sb.Append("Удалена сущность \"");

            // Получение названия типа сущности
            sb.Append(deletedEntityData.EntityName);

            // Получение названий и значений свойств
            if (deletedEntityData.Count > 0)
            {
                sb.Append("\". Значения: ");
                FillProperties(sb, deletedEntityData);
            }
            sb.Append('.');

            await AddHistoryAsync(sb.ToString());
        }

        public async Task WriteUpdateOperationAsync(EntityData previousEntityData, EntityData newEntityData)
        {
            StringBuilder sb = new();

            // Тип операции:
            sb.Append("Изменена сущность \"");
            // Получение названия типа сущности
            sb.Append(previousEntityData.EntityName);
            sb.Append("\".");
            // Оставить только отличающиеся значения, чтобы было легче ориентироваться
            // и чтобы сэкономить память
           RemoveDublicates(previousEntityData, newEntityData);

            // Получение названий и значений старых свойств сущности
            if (previousEntityData.Count > 0)
            {
                sb.Append(" Прошлые значения: ");
                FillProperties(sb, previousEntityData);
                sb.Append('.');
            }
            // Получение названий и значений новых свойств сущности
            if (newEntityData.Count > 0)
            {
                sb.Append(" Новые значения: ");
                FillProperties(sb, newEntityData);
                sb.Append('.');
            }

            await AddHistoryAsync(sb.ToString());
        }

        private static void RemoveDublicates(EntityData previousEntityData, EntityData newEntityData)
        {
            List<string> listToRemove = new();
            foreach (var oldPropertyData in previousEntityData)
            {
                var newPropertyData = newEntityData.Single(e => e.Name.Equals(oldPropertyData.Name));
                if (oldPropertyData.Value.Equals(newPropertyData.Value))
                    listToRemove.Add(oldPropertyData.Name);
            }
            if (listToRemove.Count > 0)
                foreach (var item in listToRemove)
                {
                    previousEntityData.Remove(item);
                    newEntityData.Remove(item);
                }
        }

        private static void FillProperties(StringBuilder sb, EntityData entityData)
        {
            for (int i = 0; i < entityData.Count; i++)
            {
                sb.Append(entityData[i].Name);
                sb.Append(" = ");
                sb.Append(entityData[i].Value);
                if (i < entityData.Count - 1) sb.Append(", ");
            }
        }

        private async Task AddHistoryAsync(string message)
        {
            History history = new() { Message = message };
            await historyRepository.CreateAsync(history);
        }
    }
}
