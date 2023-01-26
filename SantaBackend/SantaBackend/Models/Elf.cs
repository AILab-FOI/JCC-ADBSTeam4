namespace SantaBackEnd
{
    public partial class Elf
    {
        public Elf()
        {
            Shiftblocks = new HashSet<Shiftblock>();
            Unavailableblocks = new HashSet<Unavailableblock>();
        }

        public int Elfid { get; set; }
        public int? Workshopid { get; set; }
        public int? Roleid { get; set; }
        public int Userid { get; set; }

        public virtual Role? Role { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Workshop? Workshop { get; set; }
        public virtual ICollection<Shiftblock> Shiftblocks { get; set; }
        public virtual ICollection<Unavailableblock> Unavailableblocks { get; set; }
    }
}
