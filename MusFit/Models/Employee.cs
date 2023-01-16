using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class Employee
    {
        public Employee()
        {
            ClassIntroduces = new HashSet<ClassIntroduce>();
            ClassOrders = new HashSet<ClassOrder>();
            Classes = new HashSet<Class>();
            CoachSpecials = new HashSet<CoachSpecial>();
            News = new HashSet<News>();
        }

        public int EId { get; set; }
        public string ENumber { get; set; }
        public string EName { get; set; }
        public string EEngName { get; set; }
        public string EMail { get; set; }
        public bool EGender { get; set; }
        public DateTime EBirth { get; set; }
        public string EPhone { get; set; }
        public string EAccount { get; set; }
        public byte[] EPassword { get; set; }
        public string EContactor { get; set; }
        public string EContactorPhone { get; set; }
        public byte[] EPhoto { get; set; }
        public string EAddress { get; set; }
        public DateTime? EEnrollDate { get; set; }
        public DateTime? EResignDate { get; set; }
        public string EToken { get; set; }
        public bool EIsCoach { get; set; }
        public string EExplain { get; set; }
        public string EIdentityCard { get; set; }

        public virtual ICollection<ClassIntroduce> ClassIntroduces { get; set; }
        public virtual ICollection<ClassOrder> ClassOrders { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<CoachSpecial> CoachSpecials { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
