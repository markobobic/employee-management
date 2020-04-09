using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyCompany.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
        [ForeignKey("Role")]
        public int RoleID { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        public Role Role { get; set; }
        public Employee Employee { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
    }
}