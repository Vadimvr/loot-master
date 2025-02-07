using Microsoft.EntityFrameworkCore.Query;

namespace loot_master.Models
{
    internal class Player : ViewModels.Base.ViewModelBase
    {
        public Player()
        {

        }
        public string Name { get; set; } = default!;
        public int Id { get; set; }

        #region Ep type int -  
        private int _Ep;
        public int Ep
        {
            get => _Ep; set
            {
                if (Set(ref _Ep, value))
                {
                    SetPr();
                }
            }
        }

        private void SetPr()
        {
            OnPropertyChanged(nameof(Pr));
        }
        #endregion


        #region Gp type int -  
        private int _Gp = 1;
        public int Gp
        {
            get => _Gp;
            set
            {
                if (value <= 0) { value = 1; }
                if (Set(ref _Gp, value))
                {
                    SetPr();
                }
            }
        }
        #endregion
        public double Pr => Ep / Gp;

        public static implicit operator PlayerInRaid(Player player)
        {
            return new PlayerInRaid() { Player = player, PlayerId = player.Id, Name = player.Name, Color = "Red" };
        }
    }
}
