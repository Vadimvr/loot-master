using loot_master.Models;
using loot_master.Service.Db;
using System.Collections.ObjectModel;

namespace loot_master.Service.Data
{
    internal interface IDataService
    {
        public ObservableCollection<Player> Players { get; set; }

        void AddNewPlayer(Player pLayer);
        void DeletePlayer(int v);
        public event Action<IEnumerable<Player>>? AddPlayersInRaidEvent;
        public event Action<string, DateTime>? AddWinerInLogEvent;
        void AddWinerInLog(string winerName, DateTime date);
        void AddPlayersInRaid(IEnumerable<Player> players);
        public ApplicationDb db { get; }
    }
    internal class DataService : IDataService
    {
        ObservableCollection<Player> players;
        public DataService()
        {
            db = new ApplicationDb();
            db.Database.EnsureCreated();
            players = new ObservableCollection<Player>(db.Players);
        }
        public void AddNewPlayer(Player player)
        {
            var x = db.Players.Add(player).Entity;
            db.SaveChanges();
            Players.Add(x);
        }
        public void DeletePlayer(int id)
        {
            var player = db.Players.FirstOrDefault(p => p.Id == id);
            if (player != null)
            {
                db.Players.Remove(player);
                db.SaveChanges();
                Players.Remove(player);
            }
        }
        public event Action<IEnumerable<Player>>? AddPlayersInRaidEvent;
        public event Action<string, DateTime>? AddWinerInLogEvent;
        public void AddPlayersInRaid(IEnumerable<Player> players)
        {
            AddPlayersInRaidEvent?.Invoke(players);
        }
        public void AddWinerInLog(string winerName, DateTime date)
        {
            AddWinerInLogEvent?.Invoke(winerName, date);
        }
        public ObservableCollection<Player> Players { get => players; set => players = value; }
        public ApplicationDb db { get; private set; } = new();
    }
}
