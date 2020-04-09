namespace MyCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Education : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Education", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Education");
        }
    }
}
