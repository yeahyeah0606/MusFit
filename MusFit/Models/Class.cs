using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassTimes = new HashSet<ClassTime>();
        }

        public int CId { get; set; }
        public string CNumber { get; set; }
        public string CName { get; set; }
        public int LcId { get; set; }
        public int Cprice { get; set; }
        public int EId { get; set; }
        public int RoomId { get; set; }
        public short CActual { get; set; }
        public short CExpect { get; set; }
        public short CTotalLession { get; set; }

        public virtual Employee EIdNavigation { get; set; }
        public virtual LessionCategory Lc { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<ClassTime> ClassTimes { get; set; }
    }
}
