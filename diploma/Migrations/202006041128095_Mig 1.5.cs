namespace diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCourses", "Confirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserCourses", "Confirmed");
        }
    }
}
