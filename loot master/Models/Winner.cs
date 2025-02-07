namespace loot_master.Models
{
    internal class Winner
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }

}
