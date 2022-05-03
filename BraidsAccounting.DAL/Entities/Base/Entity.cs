using BraidsAccounting.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BraidsAccounting.DAL.Entities.Base;

public abstract class Entity : IEntity, INotifyPropertyChanged
{
    public int Id { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}   
