namespace MyCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrentUserRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CurrentRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentRoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CurrentRoles");
        }
    }
}
