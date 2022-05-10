using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces;

internal interface IHistoryService
{
    Task<List<History>> GetAllAsync();
    Task WriteCreateOperationAsync(EntityData createdEntityData);
    Task WriteDeleteOperationAsync(EntityData deletedEntityData);
    Task WriteUpdateOperationAsync(EntityData previousEntityData, EntityData newEntityData);
}
