namespace MyCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Employees", name: "ClientSector_Id", newName: "ClientSectorID");
            RenameIndex(table: "dbo.Employees", name: "IX_ClientSector_Id", newName: "IX_ClientSectorID");
            AddColumn("dbo.Employees", "SectorID", c => c.Int());
            CreateIndex("dbo.Employees", "SectorID");
            AddForeignKey("dbo.Employees", "SectorID", "dbo.Sectors", "SectorID");
            DropColumn("dbo.ClientSectors", "EmployeeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientSectors", "EmployeeID", c => c.Int());
            DropForeignKey("dbo.Employees", "SectorID", "dbo.Sectors");
            DropIndex("dbo.Employees", new[] { "SectorID" });
            DropColumn("dbo.Employees", "SectorID");
            RenameIndex(table: "dbo.Employees", name: "IX_ClientSectorID", newName: "IX_ClientSector_Id");
            RenameColumn(table: "dbo.Employees", name: "ClientSectorID", newName: "ClientSector_Id");
        }
    }
}
