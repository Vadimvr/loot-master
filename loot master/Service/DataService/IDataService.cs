using System.Collections.ObjectModel;

namespace loot_master.Service.Data
{
    internal interface IDataService
    {
        public ObservableCollection<Player> Players { get; set; }

        void AddNewPlayer(Player pLayer);
        void DeletePlayer(int v);
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
        public ObservableCollection<Player> Players { get => players; set => players = value; }
    }

    internal class Player
    {
        public string? Name { get; set; }
        public int Id { get; set; }
    }
}
