namespace diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BalanceHistories", "Course_Id", c => c.Int());
            CreateIndex("dbo.BalanceHistories", "Course_Id");
            AddForeignKey("dbo.BalanceHistories", "Course_Id", "dbo.Courses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BalanceHistories", "Course_Id", "dbo.Courses");
            DropIndex("dbo.BalanceHistories", new[] { "Course_Id" });
            DropColumn("dbo.BalanceHistories", "Course_Id");
        }
    }
}
