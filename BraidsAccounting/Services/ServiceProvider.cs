﻿using BraidsAccounting.DAL.Entities;
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
        //private readonly IRepository<WastedItem> wastedItemsRepository;

        public ServiceProvider(
            IRepository<Service> services
            , IStoreService store
            //, IRepository<WastedItem> wastedItemsRepository
            )
        {
            this.services = services;
            this.store = store;
            //this.wastedItemsRepository = wastedItemsRepository;
        }

        public void ProvideService(IEnumerable<ServiceFormItem> serviceFormItems, string name, decimal profit)
        {
            // Опеределить какие товары были использованы
            List<WastedItem> wastedItems = FillWastedItems(serviceFormItems);

            // Создать и заполнить класс оказанной услуги (Service)
            Service newService = new()
            {
                Profit = profit,
                NetProfit = CalculateNetProfit(wastedItems, profit),
                Name = name,
                WastedItems = wastedItems
            };

            // Добавить услугу в БД
            services.Create(newService);

            // Убрать использованные товары со склада
            store.RemoveItems(wastedItems);
        }

        private static List<WastedItem> FillWastedItems(IEnumerable<ServiceFormItem> serviceFormItems)
        {
            List<WastedItem> wastedItems = new();
            foreach (var formItem in serviceFormItems)
            {
                WastedItem wastedItem = new()
                {
                    Item = formItem.StoreItem.Item,
                    Count = formItem.Count
                };
                wastedItems.Add(wastedItem);
            }
            return wastedItems;
        }

        private static decimal CalculateNetProfit(IEnumerable<WastedItem> wastedItems, decimal profit)
        {
            decimal expenses = 0;
            foreach (var wastedItem in wastedItems)
                expenses += wastedItem.Item.ItemPrice.Price * wastedItem.Count;
            return profit - expenses;
        }


        //public void ProvideService(Service service)
        //{
        //    foreach (var wastedItem in service.WastedItems)
        //    {
        //        var existingStoreItem = store.Items.FirstOrDefault(si => si.Item.Id == wastedItem.Item.Id);
        //        if (existingStoreItem is null) throw new Exception("Выбранного товара нет на складе");
        //        existingStoreItem.Count -= wastedItem.Count;
        //        if (existingStoreItem.Count > 0)
        //            store.Edit(existingStoreItem);
        //        else if (existingStoreItem.Count == 0)
        //            store.Delete(existingStoreItem.Id);
        //        else
        //            throw new Exception("Отсутсвует требуемое количество материала на складе.");

        //        wastedItemsRepository.Create(wastedItem);
        //    }
        //    services.Create(service);

        //}
    }
}
