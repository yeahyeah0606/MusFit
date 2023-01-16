using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class Room
    {
        public Room()
        {
            Classes = new HashSet<Class>();
            RoomStates = new HashSet<RoomState>();
        }

        public int RoomId { get; set; }
        public int? RoomName { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<RoomState> RoomStates { get; set; }
    }
}
