using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraidsAccounting.Services;

/// <summary>
/// Реализация сервиса <see cref = "IManufacturersService" />.
/// </summary>
internal class ManufacturersService : IManufacturersService, IHistoryTracer<Manufacturer>
{
    private readonly IRepository<Manufacturer> manufacturers;
    private readonly IHistoryService historyService;

    public ManufacturersService(IRepository<Manufacturer> manufacturers, IHistoryService historyService)
    {
        this.manufacturers = manufacturers;
        this.historyService = historyService;
    }

    public async Task<List<Manufacturer>> GetAllAsync() =>
        await manufacturers.Items.ToListAsync();
    public async Task<List<string>> GetNamesAsync() =>
        await manufacturers.Items.Select(m => m.Name).ToListAsync();
    public async Task<Manufacturer?> GetAsync(string name) =>
        await manufacturers.Items.FirstOrDefaultAsync(m => m.Name.ToUpper() == name.ToUpper());

    public decimal GetPrice(string name) => 
        manufacturers.Items.Where(m => m.Name == name).Select(m => m.Price).SingleOrDefault();    

    public async Task AddAsync(Manufacturer? manufacturer)
    {
        if (manufacturer is null) throw new ArgumentNullException(nameof(manufacturer));
        await manufacturers.CreateAsync(manufacturer);
        await historyService.WriteCreateOperationAsync(manufacturer.GetEtityData(this));
    }

    public async Task EditAsync(Manufacturer? manufacturer)
    {
        if (manufacturer is null) throw new ArgumentNullException(nameof(manufacturer));
        var existingManufacturer = manufacturers.Get(manufacturer.Id) with { };
        await manufacturers.EditAsync(manufacturer);
        await historyService.WriteUpdateOperationAsync(existingManufacturer.GetEtityData(this), manufacturer.GetEtityData(this));
    }

    public async Task RemoveAsync(int id)
    {
        var catalogue = ServiceLocator.GetService<ICatalogueService>();
        if (catalogue.ContainsManufacturer(id))
            throw new ArgumentException(Messages.ManufacturerUsedInCatalogue, nameof(id));
        var existingManufacturer = await manufacturers.GetAsync(id);
        await manufacturers.RemoveAsync(id);
        await historyService.WriteDeleteOperationAsync(existingManufacturer.GetEtityData(this));
    }

    IEntityDataBuilder<Manufacturer> IHistoryTracer<Manufacturer>.ConfigureEntityData(IEntityDataBuilder<Manufacturer> builder, Manufacturer entity) =>
        builder.AddInfo(e => e.Name, entity.Name);
}
