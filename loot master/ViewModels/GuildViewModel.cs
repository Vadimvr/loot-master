using loot_master.Commands.Base;
using loot_master.Models;
using loot_master.Service.Data;
using loot_master.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace loot_master.ViewModels
{
    internal class GuildViewModel : ViewModelBase
    {
        const int BaseEp = 5000;
        const int BaseGp = 500;

        #region ViewName type string -  
        private string _ViewName = "Игроки в гильдии";
        private readonly IDataService _dataService;

        public string ViewName { get => _ViewName; set => Set(ref _ViewName, value); }
        #endregion

        public GuildViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }
        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => LambdaCommand.CallAllCanExecute();

        #region PLayers type ObservableCollection<string> -  
        public ObservableCollection<Player> Players { get => _dataService.Players; set => _dataService.Players = value; }
        #endregion

        #region SelectedPLayers type ObservableCollection<string> -  
        private IList<object> _SelectedPlayers = new List<object>();
        public IList<object> SelectedPlayers
        {
            get => _SelectedPlayers;
            set => Set(ref _SelectedPlayers, value);
        }
        #endregion

        #region SelectedPlayersCommand - описание команды 
        private LambdaCommand? _SelectedPlayersCommand;
        public ICommand SelectedPlayersCommand => _SelectedPlayersCommand ??=
            new LambdaCommand(OnSelectedPlayersCommandExecuted, CanSelectedPlayersCommandExecute);
        private bool CanSelectedPlayersCommandExecute(object? p) => true;
        private void OnSelectedPlayersCommandExecuted(object? p)
        {
            LambdaCommand.CallAllCanExecute();
        }
        #endregion

        #region AddSelectedPlayersInRaidCommand - описание команды 
        private LambdaCommand? _AddSelectedPlayersInRaidCommand;
        public ICommand AddSelectedPlayersInRaidCommand => _AddSelectedPlayersInRaidCommand ??=
            new LambdaCommand(OnAddSelectedPlayersInRaidCommandExecuted, CanAddSelectedPlayersInRaidCommandExecute, true);
        private bool CanAddSelectedPlayersInRaidCommandExecute(object? p) => SelectedPlayers.Count > 0;
        private void OnAddSelectedPlayersInRaidCommandExecuted(object? p)
        {
            List<Player> selectedPlayers = new List<Player>();
            Player? player;
            foreach (var item in SelectedPlayers)
            {
                if (item is Player && (player = item as Player) != null)
                {
                    selectedPlayers.Add(player);
                }
            }
            _dataService.AddPlayersInRaid(selectedPlayers);
            SelectedPlayers.Clear();
        }
        #endregion

        #region DeleteSelectPlayerCommand - описание команды 
        private LambdaCommand? _DeleteSelectPlayerCommand;
        public ICommand DeleteSelectPlayerCommand => _DeleteSelectPlayerCommand ??=
            new LambdaCommand(OnDeleteSelectPlayerCommandExecuted, CanDeleteSelectPlayerCommandExecute);
        private bool CanDeleteSelectPlayerCommandExecute(object? p) => true;
        private void OnDeleteSelectPlayerCommandExecuted(object? p)
        {
            if (p is int)
            {
                int x = (int)p;
                _dataService.DeletePlayer(x);
            }
        }
        #endregion

        #region AddNewPlayerCommand - описание команды 
        private LambdaCommand? _AddNewPlayerCommand;
        public ICommand AddNewPlayerCommand => _AddNewPlayerCommand ??=
            new LambdaCommand(OnAddNewPlayerCommandExecuted, CanAddNewPlayerCommandExecute, true);
        private bool CanAddNewPlayerCommandExecute(object? p) => !string.IsNullOrEmpty(_NewPlayerName);
        private void OnAddNewPlayerCommandExecuted(object? p)
        {
            var player = _dataService.Players.FirstOrDefault(x => x.Name == NewPlayerName);
            if (player is null)
            {
                _dataService.AddNewPlayer(new Player() { Name = NewPlayerName, Ep = BaseEp, Gp = BaseGp });
                NewPlayerName = string.Empty;
            }
            else
            {
                Task.Run(() =>
                {
                    App.Alert.ShowAlert("Error", $"Player is exist: {NewPlayerName}");
                });
            }

        }
        #endregion

        #region NewPlayerName type string -  
        private string _NewPlayerName = default!;
        public string NewPlayerName
        {
            get => _NewPlayerName; set
            {
                if (Set(ref _NewPlayerName, value))
                {
                    LambdaCommand.CallAllCanExecute();
                }
            }
        }
        #endregion
    }
}
