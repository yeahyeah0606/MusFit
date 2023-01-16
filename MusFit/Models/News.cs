using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class News
    {
        public int NId { get; set; }
        public string NTitle { get; set; }
        public string NContent { get; set; }
        public string NCategory { get; set; }
        public DateTime NPostDate { get; set; }
        public byte[] NPhoto { get; set; }
        public int EId { get; set; }
        public DateTime? NTakeDownTime { get; set; }

        public virtual Employee EIdNavigation { get; set; }
    }
}
