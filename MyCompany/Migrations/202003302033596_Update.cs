namespace MyCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "EmployeeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "EmployeeID");
            AddForeignKey("dbo.Users", "EmployeeID", "dbo.Employees", "EmployeeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.Users", new[] { "EmployeeID" });
            DropColumn("dbo.Users", "EmployeeID");
        }
    }
}
