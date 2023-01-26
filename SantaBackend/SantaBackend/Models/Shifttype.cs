using System;
using System.Collections.Generic;

namespace SantaBackEnd
{
    public partial class Shifttype
    {
        public Shifttype()
        {
            Shiftblocks = new HashSet<Shiftblock>();
        }

        public int Shifttypeid { get; set; }
        public string Type { get; set; } = null!;
        public TimeOnly Starttime { get; set; }
        public TimeOnly Endtime { get; set; }

        public virtual ICollection<Shiftblock> Shiftblocks { get; set; }
    }
}
