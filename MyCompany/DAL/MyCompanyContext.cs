using MyCompany.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MyCompany.DAL
{
    public class MyCompanyContext : DbContext
    {
        public MyCompanyContext() : base("MyCompanyDB")
        {

        }

        public DbSet<ClientSector> ClientSectors { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<EmployeeEnrollment> EmployeeEnrollments { get; set; }
        public DbSet<TeamLead> TeamLeads { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CurrentRole> CurrentRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().Ignore(x => x.ImagePath);
        }
    }
}