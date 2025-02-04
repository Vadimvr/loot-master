namespace loot_master.Models
{
    internal class Player
    {
        public string? Name { get; set; }
        public int Id { get; set; }

        public static implicit operator PlayerInRaid(Player player)
        {
            return new PlayerInRaid() { Id = player.Id, Name = player.Name, Color = "Red" };
        }
    }
}
