using BraidsAccounting.Services.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.ViewModels
{
    internal class StatisticsViewModel : BindableBase
    {
        private readonly Services.Interfaces.IServiceProvider services;

        public StatisticsViewModel(
            Services.Interfaces.IServiceProvider services
            )
        {
            this.services = services;
        }

        public string Name { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }



    }
}
