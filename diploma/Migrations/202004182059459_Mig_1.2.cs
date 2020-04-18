namespace diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BalanceHistories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Sum = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BalanceHistories", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.BalanceHistories", new[] { "User_Id" });
            DropTable("dbo.BalanceHistories");
        }
    }
}
