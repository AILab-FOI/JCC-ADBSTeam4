using System;
using System.Collections.Generic;

namespace SantaBackEnd
{
    public partial class Role
    {
        public Role()
        {
            Elves = new HashSet<Elf>();
        }

        public int Roleid { get; set; }
        public string Role1 { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Elf> Elves { get; set; }
    }
}
