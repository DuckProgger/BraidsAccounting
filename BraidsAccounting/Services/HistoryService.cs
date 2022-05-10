using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
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

        public async Task WriteCreateOperationAsync(EntityData createdEntityData)
        {
            StringBuilder sb = new();

            // Тип операции:
            sb.Append("Добавлена сущность \"");

            // Получение названия типа сущности
            sb.Append(createdEntityData.EntityName);

            // Получение названий и значений свойств
            sb.Append("\". Значения: ");
            FillProperties(sb, createdEntityData);
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
            sb.Append("\". Значения: ");
            FillProperties(sb, deletedEntityData);
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

            // Получение названий и значений старых свойств сущности
            sb.Append("\". Прошлые значения: ");
            FillProperties(sb, previousEntityData);

            // Получение названий и значений новых свойств сущности
            sb.Append("\". Новые значения: ");
            FillProperties(sb, newEntityData);
            sb.Append('.');

            await AddHistoryAsync(sb.ToString());
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

        //public async Task AddAsync(OperationType operationType, string comment)
        //{
        //    History history = new();
        //    history.Message = $"Операция: \"{GetLocalizedOperationName(operationType)}\", " +
        //        $"Сущность: \"{GetLocalizedEntityName()}\" {comment}";
        //    await historyRepository.CreateAsync(history);
        //}



        //private static string GetLocalizedEntityName() =>
        //    new T().GetLocalizedName();

        //private static string GetLocalizedOperationName(OperationType operationType) =>
        //    operationType switch
        //    {
        //        OperationType.Add => "Добавление",
        //        OperationType.Update => "Изменение",
        //        OperationType.Remove => "Удаление",
        //        _ => throw new NotImplementedException("Неизвестный тип операции."),
        //    };
    }
    //public enum OperationType { Add, Update, Remove }
}
