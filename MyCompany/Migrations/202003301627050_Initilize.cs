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
                        ClientSectorID = c.Int(nullable: false),
                        Title = c.String(maxLength: 50),
                        Budget = c.Int(nullable: false),
                        SectorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientSectorID)
                .ForeignKey("dbo.Sectors", t => t.SectorID, cascadeDelete: true)
                .Index(t => t.SectorID);
            
            CreateTable(
                "dbo.EmployeeEnrollments",
                c => new
                    {
                        EnrollmentID = c.Int(nullable: false, identity: true),
                        ClientSectorID = c.Int(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                        Level = c.Int(),
                    })
                .PrimaryKey(t => t.EnrollmentID)
                .ForeignKey("dbo.ClientSectors", t => t.ClientSectorID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.ClientSectorID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        EnrollmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .Index(t => t.LastName)
                .Index(t => t.FirstName)
                .Index(t => t.EnrollmentDate);
            
            CreateTable(
                "dbo.Sectors",
                c => new
                    {
                        SectorID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Budget = c.Decimal(nullable: false, storeType: "money"),
                        StartDate = c.DateTime(nullable: false),
                        TeamLeadID = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SectorID)
                .ForeignKey("dbo.TeamLeads", t => t.TeamLeadID)
                .Index(t => t.TeamLeadID);
            
            CreateTable(
                "dbo.TeamLeads",
                c => new
                    {
                        TeamLeadID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        HireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TeamLeadID);
            
            CreateTable(
                "dbo.OfficeAssignments",
                c => new
                    {
                        TeamLeadID = c.Int(nullable: false),
                        Location = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TeamLeadID)
                .ForeignKey("dbo.TeamLeads", t => t.TeamLeadID)
                .Index(t => t.TeamLeadID);
            
            CreateTable(
                "dbo.ClientSectorTeamLead",
                c => new
                    {
                        ClientSectorID = c.Int(nullable: false),
                        TeamLeadID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClientSectorID, t.TeamLeadID })
                .ForeignKey("dbo.ClientSectors", t => t.ClientSectorID, cascadeDelete: true)
                .ForeignKey("dbo.TeamLeads", t => t.TeamLeadID, cascadeDelete: true)
                .Index(t => t.ClientSectorID)
                .Index(t => t.TeamLeadID);
            
            CreateStoredProcedure(
                "dbo.Sector_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Budget = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        TeamLeadID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Sectors]([Name], [Budget], [StartDate], [TeamLeadID])
                      VALUES (@Name, @Budget, @StartDate, @TeamLeadID)
                      
                      DECLARE @SectorID int
                      SELECT @SectorID = [SectorID]
                      FROM [dbo].[Sectors]
                      WHERE @@ROWCOUNT > 0 AND [SectorID] = scope_identity()
                      
                      SELECT t0.[SectorID], t0.[RowVersion]
                      FROM [dbo].[Sectors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[SectorID] = @SectorID"
            );
            
            CreateStoredProcedure(
                "dbo.Sector_Update",
                p => new
                    {
                        SectorID = p.Int(),
                        Name = p.String(maxLength: 50),
                        Budget = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        TeamLeadID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Sectors]
                      SET [Name] = @Name, [Budget] = @Budget, [StartDate] = @StartDate, [TeamLeadID] = @TeamLeadID
                      WHERE (([SectorID] = @SectorID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Sectors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[SectorID] = @SectorID"
            );
            
            CreateStoredProcedure(
                "dbo.Sector_Delete",
                p => new
                    {
                        SectorID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Sectors]
                      WHERE (([SectorID] = @SectorID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Sector_Delete");
            DropStoredProcedure("dbo.Sector_Update");
            DropStoredProcedure("dbo.Sector_Insert");
            DropForeignKey("dbo.ClientSectorTeamLead", "TeamLeadID", "dbo.TeamLeads");
            DropForeignKey("dbo.ClientSectorTeamLead", "ClientSectorID", "dbo.ClientSectors");
            DropForeignKey("dbo.ClientSectors", "SectorID", "dbo.Sectors");
            DropForeignKey("dbo.Sectors", "TeamLeadID", "dbo.TeamLeads");
            DropForeignKey("dbo.OfficeAssignments", "TeamLeadID", "dbo.TeamLeads");
            DropForeignKey("dbo.EmployeeEnrollments", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeEnrollments", "ClientSectorID", "dbo.ClientSectors");
            DropIndex("dbo.ClientSectorTeamLead", new[] { "TeamLeadID" });
            DropIndex("dbo.ClientSectorTeamLead", new[] { "ClientSectorID" });
            DropIndex("dbo.OfficeAssignments", new[] { "TeamLeadID" });
            DropIndex("dbo.Sectors", new[] { "TeamLeadID" });
            DropIndex("dbo.Employees", new[] { "EnrollmentDate" });
            DropIndex("dbo.Employees", new[] { "FirstName" });
            DropIndex("dbo.Employees", new[] { "LastName" });
            DropIndex("dbo.EmployeeEnrollments", new[] { "EmployeeID" });
            DropIndex("dbo.EmployeeEnrollments", new[] { "ClientSectorID" });
            DropIndex("dbo.ClientSectors", new[] { "SectorID" });
            DropTable("dbo.ClientSectorTeamLead");
            DropTable("dbo.OfficeAssignments");
            DropTable("dbo.TeamLeads");
            DropTable("dbo.Sectors");
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeEnrollments");
            DropTable("dbo.ClientSectors");
        }
    }
}
