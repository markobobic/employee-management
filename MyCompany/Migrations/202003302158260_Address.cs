namespace MyCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Address : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "LivingAddress", c => c.String());
            AddColumn("dbo.Employees", "AddressFromCard", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "AddressFromCard");
            DropColumn("dbo.Employees", "LivingAddress");
        }
    }
}
