using System;
using System.Collections.Generic;

namespace SantaBackEnd
{
    public partial class Unavailableblock
    {
        public int Blockid { get; set; }
        public int Elfid { get; set; }
        public DateTime Starttime { get; set; }
        public DateTime Endtime { get; set; }
        public string? Note { get; set; }

        public virtual Elf Elf { get; set; } = null!;
    }
}
