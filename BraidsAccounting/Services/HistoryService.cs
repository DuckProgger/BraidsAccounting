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

            // Получение названий и значений свойств
            if (createdEntityData.Count > 0)
                FillProperties(sb, createdEntityData);
            sb.Append('.');

            History history = new()
            {
                EntityName = createdEntityData.EntityName,
                Operation = "Добавление",
                Message = sb.ToString()
            };

            await AddHistoryAsync(history);
        }

        public async Task WriteDeleteOperationAsync(EntityData deletedEntityData)
        {
            StringBuilder sb = new();   

            // Получение названий и значений свойств
            if (deletedEntityData.Count > 0)
                FillProperties(sb, deletedEntityData);
            sb.Append('.');

            History history = new()
            {
                EntityName = deletedEntityData.EntityName,
                Operation = "Удаление",
                Message = sb.ToString()
            };

            await AddHistoryAsync(history);
        }

        public async Task WriteUpdateOperationAsync(EntityData previousEntityData, EntityData newEntityData)
        {
            // Если сущность не изменилась - ничего не писать
            if (previousEntityData.Equals(newEntityData)) return;            

            StringBuilder sb = new();          

            // Получение названий и значений старых свойств сущности
            sb.Append("Прошлые значения: ");
            FillProperties(sb, previousEntityData);

            // Получение названий и значений новых свойств сущности
            sb.Append(".\nНовые значения: ");
            FillProperties(sb, newEntityData);
            sb.Append('.');

            History history = new()
            {
                EntityName = newEntityData.EntityName,
                Operation = "Изменение",
                Message = sb.ToString()
            };

            await AddHistoryAsync(history);
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

        private async Task AddHistoryAsync(History history)
        {
            await historyRepository.CreateAsync(history);
        }
    }
}
