namespace MyCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initilize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientSectors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SectorID = c.Int(nullable: false),
                        EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        PhotoType = c.String(),
                        Photo = c.Binary(),
                        EnrollmentDate = c.DateTime(nullable: false),
                        LivingAddress = c.String(),
                        AddressFromCard = c.String(),
                        Education = c.String(),
                        Mobile = c.String(),
                        WorkPhone = c.String(),
                        ReportsTo = c.String(),
                        OfficialWorkStart = c.DateTime(nullable: false),
                        WorkStart = c.DateTime(nullable: false),
                        WorkEmail = c.String(),
                        PersonalEmail = c.String(),
                        TeamLeadID = c.Int(),
                        ClientSector_Id = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.TeamLeads", t => t.TeamLeadID)
                .ForeignKey("dbo.ClientSectors", t => t.ClientSector_Id)
                .Index(t => t.LastName)
                .Index(t => t.FirstName)
                .Index(t => t.EnrollmentDate)
                .Index(t => t.OfficialWorkStart)
                .Index(t => t.WorkStart)
                .Index(t => t.TeamLeadID)
                .Index(t => t.ClientSector_Id);
            
            CreateTable(
                "dbo.EmployeeEnrollments",
                c => new
                    {
                        EnrollmentID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        Level = c.Int(),
                    })
                .PrimaryKey(t => t.EnrollmentID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.TeamLeads",
                c => new
                    {
                        TeamLeadID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.TeamLeadID);
            
            CreateTable(
                "dbo.Sectors",
                c => new
                    {
                        SectorID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClientSectorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SectorID);
            
            CreateTable(
                "dbo.CurrentRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentRoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OfficeAssignments",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false),
                        Location = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        RoleID = c.Int(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.ClientSectorSectors",
                c => new
                    {
                        ClientSector_Id = c.Int(nullable: false),
                        Sector_SectorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClientSector_Id, t.Sector_SectorID })
                .ForeignKey("dbo.ClientSectors", t => t.ClientSector_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sectors", t => t.Sector_SectorID, cascadeDelete: true)
                .Index(t => t.ClientSector_Id)
                .Index(t => t.Sector_SectorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Users", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.OfficeAssignments", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.ClientSectorSectors", "Sector_SectorID", "dbo.Sectors");
            DropForeignKey("dbo.ClientSectorSectors", "ClientSector_Id", "dbo.ClientSectors");
            DropForeignKey("dbo.Employees", "ClientSector_Id", "dbo.ClientSectors");
            DropForeignKey("dbo.Employees", "TeamLeadID", "dbo.TeamLeads");
            DropForeignKey("dbo.EmployeeEnrollments", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.ClientSectorSectors", new[] { "Sector_SectorID" });
            DropIndex("dbo.ClientSectorSectors", new[] { "ClientSector_Id" });
            DropIndex("dbo.Users", new[] { "EmployeeID" });
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.OfficeAssignments", new[] { "EmployeeID" });
            DropIndex("dbo.EmployeeEnrollments", new[] { "EmployeeID" });
            DropIndex("dbo.Employees", new[] { "ClientSector_Id" });
            DropIndex("dbo.Employees", new[] { "TeamLeadID" });
            DropIndex("dbo.Employees", new[] { "WorkStart" });
            DropIndex("dbo.Employees", new[] { "OfficialWorkStart" });
            DropIndex("dbo.Employees", new[] { "EnrollmentDate" });
            DropIndex("dbo.Employees", new[] { "FirstName" });
            DropIndex("dbo.Employees", new[] { "LastName" });
            DropTable("dbo.ClientSectorSectors");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.OfficeAssignments");
            DropTable("dbo.CurrentRoles");
            DropTable("dbo.Sectors");
            DropTable("dbo.TeamLeads");
            DropTable("dbo.EmployeeEnrollments");
            DropTable("dbo.Employees");
            DropTable("dbo.ClientSectors");
        }
    }
}
