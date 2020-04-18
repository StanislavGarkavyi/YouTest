namespace diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BalanceHistories", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BalanceHistories", "Comment");
        }
    }
}
