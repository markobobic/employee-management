using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompany.Models
{
    public class Sector
    {
        [Key]
        public int SectorID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }
        [ForeignKey("ClientSectors")]
        public int ClientSectorID { get; set; }
        public virtual ICollection<ClientSector> ClientSectors { get; set; }

        public Sector()
        {
            this.ClientSectors = new HashSet<ClientSector>();
        }
    }
}