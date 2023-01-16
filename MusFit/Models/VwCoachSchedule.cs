using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class VwCoachSchedule
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string ClassTitle { get; set; }
        public int CoachId { get; set; }
        public string CoachName { get; set; }
        public string ClassName { get; set; }
        public short ClassLession { get; set; }
        public int? RoomNumber { get; set; }
        public string Weekday { get; set; }
        public string ClassStart { get; set; }
        public string ClassEnd { get; set; }
        public short StudentNumber { get; set; }
        public string Color { get; set; }
    }
}
