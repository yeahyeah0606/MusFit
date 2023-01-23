using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

#nullable disable

namespace MusFit.Models
{
    public partial class Student
    {
        public Student()
        {
            ClassOrders = new HashSet<ClassOrder>();
            ClassRecords = new HashSet<ClassRecord>();
            InBodies = new HashSet<InBody>();
        }

        public int SId { get; set; }

        
        public string SNumber { get; set; }
       
        public string SName { get; set; }

      
        public string SMail { get; set; }

      
        public DateTime? SBirth { get; set; }

      
        public bool SGender { get; set; }

       
        public string SContactor { get; set; }

       
        public string SContactPhone { get; set; }

        
        public string SPhoto { get; set; }

      
        public string SAddress { get; set; }

       
        public string SPhone { get; set; }

       
        public string SAccount { get; set; }
        public byte[] SPassword { get; set; }
        public string SToken { get; set; }

       
        public DateTime? SJoinDate { get; set; }

    
        public bool? SIsStudentOrNot { get; set; }

        public virtual ICollection<ClassOrder> ClassOrders { get; set; }
        public virtual ICollection<ClassRecord> ClassRecords { get; set; }
        public virtual ICollection<InBody> InBodies { get; set; }
    }
}
