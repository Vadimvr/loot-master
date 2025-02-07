using loot_master.Commands.Base;
using loot_master.Models;
using loot_master.Resources.StaticData;
using loot_master.Service.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace loot_master.ViewModels
{
    internal class RaidEpGpViewModel : ViewModels.Base.ViewModelBase
    {
        private readonly IDataService _dataService;
        #region ViewName type string -  
        private string _ViewName = "Ep Gp";
        public string ViewName { get => _ViewName; set => Set(ref _ViewName, value); }
        #endregion

        #region PlayersInRaid type List<PlayersInRaid> -  
        private ObservableCollection<Player> _Players = default!;
        public ObservableCollection<Player> Players { get => _Players; set => Set(ref _Players, value); }
        #endregion

        public RaidEpGpViewModel(IDataService dataService)
        {
            dataService.AddPlayersInRaidEvent += AddPlayersInRaid;
            _dataService = dataService;
            LambdaCommand.CallAllCanExecute();
            Players = new ObservableCollection<Player>();
        }

        private void AddPlayersInRaid(IEnumerable<Player> enumerable)
        {
            foreach (var item in enumerable.OrderByDescending(x => x.Pr))
            {
                Players.Add(item);
            }
        }

        #region AddGpCommand - описание команды 
        private LambdaCommand? _AddGpCommand;
        public ICommand AddGpCommand => _AddGpCommand ??=
            new LambdaCommand(OnAddGpCommandExecuted, CanAddGpCommandExecute);
        private bool CanAddGpCommandExecute(object? p) => true;
        private void OnAddGpCommandExecuted(object? p)
        {
            if (p is not int) { throw new ArgumentNullException(nameof(p)); }
            var player = Players.FirstOrDefault(x => x.Id == (int)p);
            if (player is not null)
            {
                player.Gp += StaticData.Gp;
                _dataService.db.Update(player);
                _dataService.AddWinerInLog(player.Name, DateTime.Now, $"+Gp {StaticData.Gp}");
                _dataService.db.SaveChanges();
                var x = Players.OrderBy(x => x.Gp).ToList();
                Players.Clear();
                foreach (var item in x)
                {
                    Players.Add(item);
                }
            }
        }
        #endregion


        #region DeleteFromRaidCommand - описание команды 
        private LambdaCommand? _DeleteFromRaidCommand;
        public ICommand DeleteFromRaidCommand => _DeleteFromRaidCommand ??=
            new LambdaCommand(OnDeleteFromRaidCommandExecuted, CanDeleteFromRaidCommandExecute);
        private bool CanDeleteFromRaidCommandExecute(object? p) => true;
        private void OnDeleteFromRaidCommandExecuted(object? p)
        {
            if (p is int)
            {
                var player = Players.FirstOrDefault(x => x.Id == (int)p);
                if (player is not null)
                {
                    Players.Remove(player);
                }
            }
        }
        #endregion


        #region AddAllEPCommand - описание команды 
        private LambdaCommand? _AddAllEPCommand;
        public ICommand AddAllEPCommand => _AddAllEPCommand ??=
            new LambdaCommand(OnAddAllEPCommandExecuted, CanAddAllEPCommandExecute);
        private bool CanAddAllEPCommandExecute(object? p) => true;
        private void OnAddAllEPCommandExecuted(object? p)
        {
            foreach (var player in Players)
            {
                player.Ep += StaticData.Ep;
                _dataService.db.Update(player);
                _dataService.AddWinerInLog(player.Name, DateTime.Now, $"+Ep {StaticData.Ep}");
            }
            _dataService.db.SaveChanges();
        }
        #endregion

    }
}
