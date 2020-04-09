namespace MyCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Employees", new[] { "EnrollmentDate" });
            DropIndex("dbo.Employees", new[] { "OfficialWorkStart" });
            AddColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Employees", "OfficialWorkStart", c => c.DateTime());
            CreateIndex("dbo.Employees", "OfficialWorkStart");
            DropColumn("dbo.Employees", "EnrollmentDate");
            DropColumn("dbo.Employees", "WorkPhone");
            DropColumn("dbo.Employees", "WorkEmail");
            DropColumn("dbo.Employees", "PersonalEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "PersonalEmail", c => c.String());
            AddColumn("dbo.Employees", "WorkEmail", c => c.String());
            AddColumn("dbo.Employees", "WorkPhone", c => c.String());
            AddColumn("dbo.Employees", "EnrollmentDate", c => c.DateTime(nullable: false));
            DropIndex("dbo.Employees", new[] { "OfficialWorkStart" });
            AlterColumn("dbo.Employees", "OfficialWorkStart", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Email");
            CreateIndex("dbo.Employees", "OfficialWorkStart");
            CreateIndex("dbo.Employees", "EnrollmentDate");
        }
    }
}
