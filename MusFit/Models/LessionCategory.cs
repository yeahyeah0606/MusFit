using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class LessionCategory
    {
        public LessionCategory()
        {
            Classes = new HashSet<Class>();
            CoachSpecials = new HashSet<CoachSpecial>();
        }

        public int LcId { get; set; }
        public string LcName { get; set; }
        public string LcThemeColor { get; set; }
        public string LcType { get; set; }
        public string LcAbbrev { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<CoachSpecial> CoachSpecials { get; set; }
    }
}
