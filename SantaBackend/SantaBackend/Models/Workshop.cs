using System;
using System.Collections.Generic;

namespace SantaBackEnd
{
    public partial class Workshop
    {
        public Workshop()
        {
            Elves = new HashSet<Elf>();
        }

        public int Workshopid { get; set; }
        public int? Santaclausid { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;

        public virtual Santaclau? Santaclaus { get; set; }
        public virtual ICollection<Elf> Elves { get; set; }
    }
}
