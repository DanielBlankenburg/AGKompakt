namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityRequestStatus",
                c => new
                    {
                        ActivityRequestStatusId = c.Int(nullable: false, identity: true),
                        ActivityRequestStatusText = c.String(),
                    })
                .PrimaryKey(t => t.ActivityRequestStatusId);
            
            AddColumn("dbo.ActivityRequests", "ActivityRequestStatusID", c => c.Int());
            AddColumn("dbo.ActivityRequestDataFiles", "FileTyp", c => c.Int(nullable: false));
            CreateIndex("dbo.ActivityRequests", "ActivityRequestStatusID");
            AddForeignKey("dbo.ActivityRequests", "ActivityRequestStatusID", "dbo.ActivityRequestStatus", "ActivityRequestStatusId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestStatusID", "dbo.ActivityRequestStatus");
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestStatusID" });
            DropColumn("dbo.ActivityRequestDataFiles", "FileTyp");
            DropColumn("dbo.ActivityRequests", "ActivityRequestStatusID");
            DropTable("dbo.ActivityRequestStatus");
        }
    }
}
