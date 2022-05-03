using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace BraidsAccounting.Models;

/// <summary>
/// Представляет класс для отображения информации о материале в форме.
/// </summary>
internal class FormItem : BindableBase/*, IEquatable<StoreItem>*/
{
    private int? maxCount;
    private int count;

    /// <summary>
    /// ID материала из каталога.
    /// </summary>
    public int ItemId { get; set; }
    /// <summary>
    /// Название производителя.
    /// </summary>
    public string Manufacturer { get; set; } = null!;
    /// <summary>
    /// Стоимость материала.
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Артикул материала.
    /// </summary>
    public string Article { get; set; } = null!;
    /// <summary>
    /// Цвет материала.
    /// </summary>
    public string Color { get; set; } = null!;
    /// <summary>
    /// Количество материалов.
    /// </summary>
    public int Count { get => count; set => count = Math.Min(value, MaxCount); }
    /// <summary>
    /// Максимально допустимое количество материалов, исходя из наличия на складе.
    /// </summary>
    public int MaxCount
    {
        get
        {
            if (maxCount is null)
            {
                // Получить сервис работы со складом
                IStoreService? store = ServiceLocator.GetService<IStoreService>();
                maxCount = store.GetItemCount(Manufacturer, Article, Color);
            }
            return maxCount.Value;
        }
    }

    //public IEnumerable<FormItem> CreateColection(IEnumerable<StoreItem> storeItems)
    //{
    //    List<FormItem> colections = new List<FormItem>();
    //    foreach (var storeItem in storeItems)
    //        colections.Add(storeItem);
    //    return colections;
    //}

    //public IEnumerable<FormItem> CreateColection(IEnumerable<Item> storeItems)
    //{
    //    List<FormItem> colections = new List<FormItem>();
    //    foreach (var storeItem in storeItems)
    //        colections.Add(storeItem);
    //    return colections;
    //}

    //public static implicit operator FormItem(StoreItem storeItem)
    //{
    //    return new()
    //    {
    //        Count = storeItem.Count,
    //        Article = storeItem.Item.Article,
    //        Manufacturer = storeItem.Item.Manufacturer.Name,
    //        Price = storeItem.Item.Manufacturer.Price,
    //        Color = storeItem.Item.Color,
    //        Id = storeItem.Item.Id
    //    };
    //}

    public static implicit operator FormItem(Item item)
    {
        return new()
        {
            Article = item.Article,
            Manufacturer = item.Manufacturer.Name,
            Price = item.Manufacturer.Price,
            Color = item.Color,
            ItemId = item.Id
        };
    }

    public static implicit operator Item(FormItem formItem)
    {
        Manufacturer manufacturer = new()
        {
            Name = formItem.Manufacturer,
            Price = formItem.Price
        };
        return new()
        {
            Id = formItem.ItemId,
            Article = formItem.Article,
            Color = formItem.Color,
            Manufacturer = manufacturer
        };
    }

    public static implicit operator WastedItem(FormItem formItem)
    {
        Manufacturer manufacturer = new()
        {
            Name = formItem.Manufacturer,
            Price = formItem.Price
        };
        Item item = new()
        {
            Id = formItem.ItemId,
            Article = formItem.Article,
            Color = formItem.Color,
            Manufacturer = manufacturer
        };
        return new WastedItem()
        {
            Count = formItem.Count,
            Item = item
        };
    }

    //public bool Equals(StoreItem? other)
    //{
    //    if (other is null) throw new ArgumentNullException(nameof(other));
    //    return
    //        Manufacturer == other.Item.Manufacturer.Name
    //        && Price == other.Item.Manufacturer.Price
    //        && Color == other.Item.Color
    //        && Article == other.Item.Article;
    //}
}
