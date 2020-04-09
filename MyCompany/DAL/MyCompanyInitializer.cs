//using MyCompany.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data;
//using EntityFramework.BulkInsert.Extensions;

//namespace MyCompany.DAL
//{
//    public class MyCompanyInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MyCompanyContext>
//    {

//        protected override void Seed(MyCompanyContext context)
//        {
//            var employees = Enumerable.Range(0, 100000)
//                   .Select(s => new Employee
//                   {
//                       FirstMidName = "Janko",
//                       LastName = "Jankovic",
//                       EnrollmentDate = DateTime.Parse("2005-09-01"),
//                   });

//            context.BulkInsert(employees, 100000);
//            context.SaveChanges();
//            var clientSectors = new List<ClientSector>
//            {
//            new ClientSector{ClientSectorID=1050,Title="Kompanija",Budget=300,},
//            new ClientSector{ClientSectorID=4022,Title="Kompanija",Budget=300,},
//            new ClientSector{ClientSectorID=4041,Title="Kompanija",Budget=300,},
//            new ClientSector{ClientSectorID=1045,Title="Kompanija",Budget=400,},
//            new ClientSector{ClientSectorID=3141,Title="Kompanija",Budget=400,},
//            new ClientSector{ClientSectorID=2021,Title="Kompanija",Budget=300,},
//            new ClientSector{ClientSectorID=2042,Title="Kompanija",Budget=400,}
//            };
//            clientSectors.ForEach(s => context.ClientSectors.Add(s));
//            context.SaveChanges();
//            var enrollments = new List<EmployeeEnrollment>
//            {
//            new EmployeeEnrollment{EmployeeID=1,ClientSectorID=1050,Level=Level.J},
//            new EmployeeEnrollment{EmployeeID=2,ClientSectorID=4022,Level=Level.J},
//            new EmployeeEnrollment{EmployeeID=3,ClientSectorID=4041,Level=Level.J},
//            new EmployeeEnrollment{EmployeeID=4,ClientSectorID=1045,Level=Level.J},
//            new EmployeeEnrollment{EmployeeID=5,ClientSectorID=3141,Level=Level.M},
//            new EmployeeEnrollment{EmployeeID=6,ClientSectorID=2021,Level=Level.J},
//            new EmployeeEnrollment{EmployeeID=7,ClientSectorID=1050},
//            new EmployeeEnrollment{EmployeeID=8,ClientSectorID=1050,},
//            new EmployeeEnrollment{EmployeeID=9,ClientSectorID=4022,Level=Level.J},
//            new EmployeeEnrollment{EmployeeID=10,ClientSectorID=4041,Level=Level.M},
//            new EmployeeEnrollment{EmployeeID=11,ClientSectorID=1045},
//            new EmployeeEnrollment{EmployeeID=12,ClientSectorID=3141,Level=Level.S}
//            };
//            enrollments.ForEach(s => context.EmployeeEnrollments.Add(s));
//            context.SaveChanges();
//        }
//    }
//}