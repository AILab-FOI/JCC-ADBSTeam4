using System;
using System.Collections.Generic;

namespace SantaBackEnd
{
    public partial class Shiftblock
    {
        public int Shiftid { get; set; }
        public int Shifttypeid { get; set; }
        public int Elfid { get; set; }
        public DateOnly Date { get; set; }

        public virtual Elf Elf { get; set; } = null!;
        public virtual Shifttype Shifttype { get; set; } = null!;
    }
}
