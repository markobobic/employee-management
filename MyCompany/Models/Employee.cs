using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompany.Models
{
    public class Employee 
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [Index]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        [Index]
        public string FirstName { get; set; }
        public string PhotoType { get; set; }
        public byte[] Photo { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Employee Enrollment Date")]
        [Index]
        public DateTime EnrollmentDate { get; set; }
        public virtual ICollection<EmployeeEnrollment> EmployeeEnrollments { get; set; }

        public string LivingAddress { get; set; }
        public string AddressFromCard { get; set; }

        public string Education { get; set; }

        public string Mobile { get; set; }
        public string WorkPhone { get; set; }
        public string ReportsTo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Employee Enrollment Date")]
        [Index]
        public DateTime OfficialWorkStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Employee Enrollment Date")]
        [Index]
        public DateTime WorkStart { get; set; }
        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        [ForeignKey("TeamLead")]
        public int? TeamLeadID { get; set; }

        public string ImagePath { get; set; }
        public virtual TeamLead TeamLead { get; set; }
        public Employee()
        {

        }

        
    }
}