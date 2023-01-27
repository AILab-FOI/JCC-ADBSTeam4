using System;
using System.Collections.Generic;

namespace SantaBackEnd
{
    public partial class Santaclau
    {
        public Santaclau()
        {
            Workshops = new HashSet<Workshop>();
        }

        public int Santaclausid { get; set; }
        public int Userid { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Workshop> Workshops { get; set; }
    }
}
