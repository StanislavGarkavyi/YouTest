namespace diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Confirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Confirmed");
        }
    }
}
