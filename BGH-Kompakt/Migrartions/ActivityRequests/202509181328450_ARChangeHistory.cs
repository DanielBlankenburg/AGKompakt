namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARChangeHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityRequestChangeHistories",
                c => new
                    {
                        ActivityRequestChangeHistoryID = c.Int(nullable: false, identity: true),
                        ActivityRequestChangeHistoryText = c.String(),
                        ActivityRequestChangeHistoryAuthor = c.String(),
                        ActivityRequestChangeHistoryDate = c.DateTime(nullable: false),
                        ActivityRequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestChangeHistoryID)
                .ForeignKey("dbo.ActivityRequests", t => t.ActivityRequestId, cascadeDelete: true)
                .Index(t => t.ActivityRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityRequestChangeHistories", "ActivityRequestId", "dbo.ActivityRequests");
            DropIndex("dbo.ActivityRequestChangeHistories", new[] { "ActivityRequestId" });
            DropTable("dbo.ActivityRequestChangeHistories");
        }
    }
}
