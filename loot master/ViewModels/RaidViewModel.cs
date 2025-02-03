using loot_master.Commands.Base;
using loot_master.Service.Data;
using loot_master.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace loot_master.ViewModels
{
    internal class RaidViewModel : ViewModelBase
    {

        #region ViewName type string -  
        private string _ViewName = "Игроки в рейде";
        public string ViewName { get => _ViewName; set => Set(ref _ViewName, value); }
        #endregion

        #region PlayersInRaid type List<PlayersInRaid> -  
        private ObservableCollection<PlayerInRaid> _Players = new ObservableCollection<PlayerInRaid>();
        public ObservableCollection<PlayerInRaid> Players { get => _Players; set => Set(ref _Players, value); }
        #endregion

        public RaidViewModel(IDataService dataService)
        {
            dataService.AddPlayersInRaidEvent += AddPlayersInRaid;
            LambdaCommand.CallAllCanExecute();
        }

        private void AddPlayersInRaid(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                if (Players.FirstOrDefault(x => x.Id == player.Id) == null)
                {
                    Players.Add((PlayerInRaid)player);
                }
            }
        }

        #region DeleteSelectPlayerCommand - описание команды 
        private LambdaCommand? _DeleteSelectPlayerCommand;
        public ICommand DeleteSelectPlayerCommand => _DeleteSelectPlayerCommand ??=
            new LambdaCommand(OnDeleteSelectPlayerCommandExecuted, CanDeleteSelectPlayerCommandExecute);
        private bool CanDeleteSelectPlayerCommandExecute(object? p) => true;
        private void OnDeleteSelectPlayerCommandExecuted(object? p)
        {
            if (p is int && (int)p > 0)
            {
                int id = (int)p;
                var player = Players.FirstOrDefault(x => x.Id == id);
                if (player != null)
                {
                    Players.Remove(player);
                }
            }
        }
        #endregion


        #region Winner type string -  
        private string _Winner = default!;
        public string Winner { get => _Winner; set => Set(ref _Winner, value); }
        #endregion


        #region GetWinerCommand - описание команды 
        private LambdaCommand? _GetWinerCommand;

        Random Random = new Random();
        public ICommand GetWinerCommand => _GetWinerCommand ??=
            new LambdaCommand(OnGetWinerCommandExecuted, CanGetWinerCommandExecute, true);
        private bool CanGetWinerCommandExecute(object? p) => Players.Count > 0;
        private void OnGetWinerCommandExecuted(object? p)
        {
            var preterpers = Players.Where(x => x.Color == "Red").ToList();

            if (preterpers.Count() == 0)
            {
                foreach (var item in Players)
                {
                    item.Color = "Red";
                }
                preterpers = Players.Where(x => x.Color == "Red").ToList();
            }

            var winer = preterpers[Random.Next(0, preterpers.Count)];
            winer.Color = "Green";
        }
        #endregion
    }
}
