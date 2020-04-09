﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompany.Models
{
   
        public enum Level
        {
            J,M,S
        }
        public class EmployeeEnrollment
        {
            [Key]
            public int EnrollmentID { get; set; }
           
            [ForeignKey("Employee")]
            public int EmployeeID { get; set; }
            [DisplayFormat(NullDisplayText = "No level")]
            public Level? Level { get; set; }

            public virtual Employee Employee { get; set; }
          

    }
}