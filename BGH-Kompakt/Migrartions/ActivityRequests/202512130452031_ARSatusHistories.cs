namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARSatusHistories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityRequestStatusHistories",
                c => new
                    {
                        ActivityRequestStatusHistoryID = c.Int(nullable: false, identity: true),
                        ActivityRequestStatusID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ActivityRequestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestStatusHistoryID)
                .ForeignKey("dbo.ActivityRequestStatus", t => t.ActivityRequestStatusID, cascadeDelete: true)
                .ForeignKey("dbo.ActivityRequests", t => t.ActivityRequestID, cascadeDelete: true)
                .Index(t => t.ActivityRequestStatusID)
                .Index(t => t.ActivityRequestID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityRequestStatusHistories", "ActivityRequestID", "dbo.ActivityRequests");
            DropForeignKey("dbo.ActivityRequestStatusHistories", "ActivityRequestStatusID", "dbo.ActivityRequestStatus");
            DropIndex("dbo.ActivityRequestStatusHistories", new[] { "ActivityRequestID" });
            DropIndex("dbo.ActivityRequestStatusHistories", new[] { "ActivityRequestStatusID" });
            DropTable("dbo.ActivityRequestStatusHistories");
        }
    }
}
