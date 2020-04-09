using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyCompany.Models
{
    public class ClientSector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Sectors")]
        public int SectorID { get; set; }
        [ForeignKey("Employees")]
        public int? EmployeeID { get; set; }
        public virtual ICollection<Sector> Sectors { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }


        public ClientSector()
        {
            this.Employees = new HashSet<Employee>();
            this.Sectors = new HashSet<Sector>();
        }
    }
}