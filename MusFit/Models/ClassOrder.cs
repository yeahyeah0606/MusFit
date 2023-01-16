using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class ClassOrder
    {
        public int OrderId { get; set; }
        public int SId { get; set; }
        public DateTime OrderTime { get; set; }
        public string OrderStatus { get; set; }
        public int? EId { get; set; }
        public int? ClassTimeId { get; set; }

        public virtual ClassTime ClassTime { get; set; }
        public virtual Employee EIdNavigation { get; set; }
        public virtual Student SIdNavigation { get; set; }
    }
}
