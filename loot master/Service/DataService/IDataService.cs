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
        public event Action<string, DateTime,string>? AddWinerInLogEvent;
        void AddWinerInLog(string winerName, DateTime date, string description = default!);
        void AddPlayersInRaid(IEnumerable<Player> players);
        void RemoveLog();
        Winner AddWinnerInLog(Winner winner);

        public ApplicationDb db { get; }
    }
    internal class DataService : IDataService
    {
        ObservableCollection<Player> players;
        public ObservableCollection<Player> Players { get => players; set => players = value; }
        public ApplicationDb db { get; private set; }
        public DataService(ApplicationDb applicationDb)
        {
            db = applicationDb;
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
        public event Action<string, DateTime, string >? AddWinerInLogEvent;
        public void AddPlayersInRaid(IEnumerable<Player> players)
        {
            AddPlayersInRaidEvent?.Invoke(players);
        }
        public void AddWinerInLog(string winerName, DateTime date, string description = default!) => AddWinerInLogEvent?.Invoke(winerName, date, description);
        public void RemoveLog()
        {
            db.SaveChanges();
            db.Winners.Select(x => db.Winners.Remove(x));
        }
        public Winner AddWinnerInLog(Winner winner)
        {
            winner = db.Winners.Add(winner).Entity;
            db.SaveChanges();
            return winner;
        }
    }
}
