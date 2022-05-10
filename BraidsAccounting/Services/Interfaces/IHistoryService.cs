using BraidsAccounting.Infrastructure;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces;

internal interface IHistoryService
{
    Task WriteCreateOperationAsync(EntityData createdEntityData);
    Task WriteDeleteOperationAsync(EntityData deletedEntityData);
    Task WriteUpdateOperationAsync(EntityData previousEntityData, EntityData newEntityData);
}
