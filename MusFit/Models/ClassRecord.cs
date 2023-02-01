using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class ClassRecord
    {
        public int CrId { get; set; }
        public int? ClassTimeId { get; set; }
        public int SId { get; set; }
        public bool? CrAttendance { get; set; }
        public string CrContent { get; set; }

        public virtual Student SIdNavigation { get; set; }
    }
}
