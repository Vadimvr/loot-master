using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loot_master.Models
{
    internal class PlayerInRaid : ViewModels.Base.ViewModelBase
    {
        [Key]
        public int IdInDb { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        #region Color type string -  
        private string _Color = "Red";
        public string Color { get => _Color; set => Set(ref _Color, value); }
        #endregion
    }
}
