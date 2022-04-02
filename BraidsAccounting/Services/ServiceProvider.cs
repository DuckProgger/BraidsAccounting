using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal class ServiceProvider : Interfaces.IServiceProvider
    {
        private readonly IRepository<Service> services;
        private readonly IStoreService store;
        private readonly IRepository<WastedItem> wastedItemsRepository;

        public ServiceProvider(
            IRepository<Service> services
            , IStoreService store
            , IRepository<WastedItem> wastedItemsRepository
            )
        {
            this.services = services;
            this.store = store;
            this.wastedItemsRepository = wastedItemsRepository;
        }

        public async Task ProvideServiceAsync(Service service)
        {
            // Добавить услугу в БД
            CalculateNetProfit(service);
            var newService = await services.CreateAsync(service);

            BindWastedItemsToService(newService);
            wastedItemsRepository.CreateRange(newService.WastedItems);

            // Убрать использованные товары со склада
            await store.RemoveItemsAsync(service.WastedItems);
        }

        /// <summary>
        /// Получить все имена сотрудников, которые когда-либо 
        /// выполняли работу
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetNames()
        {
            return services.Items.Select(s => s.WorkerName).Distinct();
        }

        private void BindWastedItemsToService(Service s)
        {
            foreach (var wastedItem in s.WastedItems)
            {
                if (wastedItem.Count <= 0) throw new ArgumentOutOfRangeException(nameof(wastedItem.Count));
                wastedItem.ServiceId = s.Id;
            }
        }

        private static void CalculateNetProfit(Service service)
        {
            decimal expenses = 0;
            foreach (var wastedItem in service.WastedItems)
                expenses += wastedItem.Item.Manufacturer.Price * wastedItem.Count;
            service.NetProfit = service.Profit - expenses;
        }

    }
}
