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
            // Если сущность не изменилась - ничего не писать
            if (previousEntityData.Equals(newEntityData)) return; 

            StringBuilder sb = new();

            // Тип операции:
            sb.Append("Изменена сущность \"");
            // Получение названия типа сущности
            sb.Append(previousEntityData.EntityName);

            // Получение названий и значений старых свойств сущности
            sb.Append("\".\nПрошлые значения: ");
            FillProperties(sb, previousEntityData);

            // Получение названий и значений новых свойств сущности
            sb.Append(".\nНовые значения: ");
            FillProperties(sb, newEntityData);
            sb.Append('.');

            await AddHistoryAsync(sb.ToString());
        }              

        private static void FillProperties(StringBuilder sb, EntityData entityData)
        {
            for (int i = 0; i < entityData.Count; i++)
            {
                sb.Append(entityData[i].Key);
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
