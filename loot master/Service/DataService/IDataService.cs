using System.Collections.ObjectModel;

namespace loot_master.Service.Data
{
    internal interface IDataService
    {
        public ObservableCollection<Player> Players { get; set; }

        void AddNewPlayer(Player pLayer);
        void DeletePlayer(int v);
        public event Action<IEnumerable<Player>> AddPlayersInRaidEvent;
        void AddPlayersInRaid(IEnumerable<Player> players);
    }
    internal class DataService : IDataService
    {
        ObservableCollection<Player> players;
        public DataService()
        {
            players = new ObservableCollection<Player>();
            for (int i = 0; i < 80; i++)
            {
                players.Add(new Player() { Name = $"Players {i}", Id = i });
            }
        }
        public void AddNewPlayer(Player player)
        {
            if (player != null)
            {
                Players.Add(player);
            }
        }
        public void DeletePlayer(int v)
        {
            List<Player> list = new List<Player>();
            foreach (var item in Players)
            {
                if (item.Id == v)
                {
                    list.Add(item);
                }
            }
            foreach (var item in list)
            {
                Players.Remove(item);
            }
        }

        public event Action<IEnumerable<Player>> AddPlayersInRaidEvent;

        public void AddPlayersInRaid(IEnumerable<Player> players)
        {
            AddPlayersInRaidEvent?.Invoke(players);
        }

        public ObservableCollection<Player> Players { get => players; set => players = value; }
    }

    internal class Player
    {
        public string? Name { get; set; }
        public int Id { get; set; }

        public static implicit operator PlayerInRaid(Player player)
        {
            return new PlayerInRaid() { Id = player.Id, Name = player.Name, Color = "Red" };
        }
    }

    internal class PlayerInRaid :ViewModels.Base.ViewModelBase
    {
        public string? Name { get; set; }
        public int Id { get; set; }


        #region Color type string -  
        private string _Color =   "Red";
        public string Color { get => _Color; set => Set(ref _Color, value); }
        #endregion
    }
}
