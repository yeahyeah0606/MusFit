using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class ClassTime
    {
        public ClassTime()
        {
            ClassOrders = new HashSet<ClassOrder>();
        }

        public int ClassTimeId { get; set; }
        public int CId { get; set; }
        public DateTime CtDate { get; set; }
        public short CtLession { get; set; }
        public int TId { get; set; }
        public string Weekday { get; set; }

        public virtual Class CIdNavigation { get; set; }
        public virtual Term TIdNavigation { get; set; }
        public virtual ICollection<ClassOrder> ClassOrders { get; set; }
    }
}
