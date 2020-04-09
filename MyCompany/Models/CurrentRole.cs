using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCompany.Models
{
    public class CurrentRole
    {
        [Key]
        public int Id { get; set; }
        public int CurrentRoleID { get; set; }
    }
}