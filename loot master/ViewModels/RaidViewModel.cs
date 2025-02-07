using loot_master.Commands.Base;
using loot_master.Models;
using loot_master.Service.Data;
using loot_master.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace loot_master.ViewModels
{
    internal class RaidViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        #region ViewName type string -  
        private string _ViewName = "Игроки в рейде";
        public string ViewName { get => _ViewName; set => Set(ref _ViewName, value); }
        #endregion

        #region PlayersInRaid type List<PlayersInRaid> -  
        private ObservableCollection<PlayerInRaid> _Players = default!;
        public ObservableCollection<PlayerInRaid> Players { get => _Players; set => Set(ref _Players, value); }
        #endregion

        public RaidViewModel(IDataService dataService)
        {
            dataService.AddPlayersInRaidEvent += AddPlayersInRaid;
            _dataService = dataService;
            LambdaCommand.CallAllCanExecute();
            Players = new ObservableCollection<PlayerInRaid>();
        }

        private void AddPlayersInRaid(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                if (Players.FirstOrDefault(x => x.PlayerId == player.Id) == null)
                {
                    PlayerInRaid playerInRaid = (PlayerInRaid)player;
                    Players.Add(playerInRaid);
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
                var player = Players.FirstOrDefault(x => x.PlayerId == id);
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
            Winner = winer.Name ?? string.Empty;
            if (!string.IsNullOrEmpty(winer.Name))
            {
                _dataService.AddWinerInLog(winer.Name, DateTime.Now, "Roll");
            }
        }
        #endregion

        #region SaveDbRaidCommand - описание команды 
        private LambdaCommand? _SaveDbRaidCommand;
        public ICommand SaveDbRaidCommand => _SaveDbRaidCommand ??=
            new LambdaCommand(OnSaveDbRaidCommandExecuted, CanSaveDbRaidCommandExecute);
        private bool CanSaveDbRaidCommandExecute(object? p) => true;
        private void OnSaveDbRaidCommandExecuted(object? p)
        {
            RemoveFromDb();
            foreach (var item in Players)
            {
                _dataService.db.InRaids.Add(item);
            }
            _dataService.db.SaveChanges();
        }
        #endregion

        #region LoadDbRaidCommand - описание команды 
        private LambdaCommand? _LoadDbRaidCommand;
        public ICommand LoadDbRaidCommand => _LoadDbRaidCommand ??=
            new LambdaCommand(OnLoadDbRaidCommandExecuted, CanLoadDbRaidCommandExecute);
        private bool CanLoadDbRaidCommandExecute(object? p) => true;
        private void OnLoadDbRaidCommandExecuted(object? p)
        {
            Players.Clear();
            foreach (var item in _dataService.db.InRaids)
            {
                Players.Add(item);
            }
        }
        #endregion

        #region ClearDbRaidCommand - описание команды 
        private LambdaCommand? _ClearDbRaidCommand;
        public ICommand ClearDbRaidCommand => _ClearDbRaidCommand ??=
            new LambdaCommand(OnClearDbRaidCommandExecuted, CanClearDbRaidCommandExecute);
        private bool CanClearDbRaidCommandExecute(object? p) => true;
        private void OnClearDbRaidCommandExecuted(object? p)
        {
            Players.Clear();
            RemoveFromDb();
        }
        #endregion

        private void RemoveFromDb()
        {
            foreach (var item in _dataService.db.InRaids)
            {
                _dataService.db.InRaids.Remove(item);
            }
            _dataService.db.SaveChanges();
        }
    }
}
