using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompany.Models
{
    public class OfficeAssignment
    {
        [Key]
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public virtual Employee Employee { get; set; }
    }
}