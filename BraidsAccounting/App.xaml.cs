using BraidsAccounting.Data;
using BraidsAccounting.Models;
using BraidsAccounting.Modules;
using BraidsAccounting.Services;
using BraidsAccounting.ViewModels;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BraidsAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .AddDatabase()
                .AddServices()
                ;
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog
                .AddModule(typeof(MainModule))
                ;
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register<AddStoreItemWindow, AddStoreItemViewModel>();
            ViewModelLocationProvider.Register<EditStoreItemWindow, EditStoreItemViewModel>();
            ViewModelLocationProvider.Register<SelectStoreItemWindow, SelectStoreItemViewModel>();
        }
    }
}
