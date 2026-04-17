namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteSingleStatus : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestStatusID", "dbo.ActivityRequestStatus");
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestStatusID" });
            DropColumn("dbo.ActivityRequests", "ActivityRequestStatusID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityRequests", "ActivityRequestStatusID", c => c.Int());
            CreateIndex("dbo.ActivityRequests", "ActivityRequestStatusID");
            AddForeignKey("dbo.ActivityRequests", "ActivityRequestStatusID", "dbo.ActivityRequestStatus", "ActivityRequestStatusId");
        }
    }
}
