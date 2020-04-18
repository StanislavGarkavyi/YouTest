namespace diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "Balance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Balance");
            DropColumn("dbo.Courses", "Price");
        }
    }
}
