using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace loot_master.ViewModels.Base
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        protected bool Set<T>(ref T filed, T value, [CallerMemberName] string? PropertyName = null)
        {
            if (Equals(filed, value)) return false;

            filed = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
