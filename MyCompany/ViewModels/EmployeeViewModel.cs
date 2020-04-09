using MyCompany.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCompany.ViewModels
{
    public class EmployeeViewModel
    {
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime WorkStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? OfficialWorkStart { get; set; }

        public string LivingAddress { get; set; }
        public string AddressFromCard { get; set; }
        public string ReportsTo { get; set; }

        public string Education { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
       
        public int RoleID { get; set; }
        public bool IsActive { get; set; }

        public bool IsTeamLead { get; set; }
        public int? TeamLeadID { get; set; }
        public int? SectorID { get; set; }
        public int? ClientSectorID { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }
        public Level Positions { get; set; }

        public enum Level
        {
            None = 0,
            [Display(Name = "Junior")]
            Junior = 1,
            [Display(Name = "Medior")]
            Medior = 2,
            [Display(Name = "Senior")]
            Senior = 3
        }

    }
}