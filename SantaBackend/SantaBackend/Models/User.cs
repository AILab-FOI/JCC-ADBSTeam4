using System;
using System.Collections.Generic;

namespace SantaBackEnd
{
    public partial class User
    {
        public User()
        {
            Elves = new HashSet<Elf>();
            Santaclaus = new HashSet<Santaclau>();
        }

        public int Userid { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateOnly Dateofbirth { get; set; }
        public DateOnly Dateofregistration { get; set; }

        public virtual ICollection<Elf> Elves { get; set; }
        public virtual ICollection<Santaclau> Santaclaus { get; set; }
    }
}
