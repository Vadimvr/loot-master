using loot_master.ViewModels.Base;

namespace loot_master.ViewModels
{
    internal class RaidViewModel: ViewModelBase
    {

        #region ViewName type string -  
        private string _ViewName = nameof(RaidViewModel);
        public string ViewName { get => _ViewName; set => Set(ref _ViewName, value); }
        #endregion

    }
}
