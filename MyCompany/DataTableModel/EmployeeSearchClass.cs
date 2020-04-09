using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCompany.DataTableModel
{
    public class EmployeeSearchClass
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime WorkStart { get; set; }

        public byte[] ProductImage { get; set; }
        public string ImagePath { get; set; }
        public string PhotoType { get; set; }
        public int filteredResultsCount { get; set; }
    }
}