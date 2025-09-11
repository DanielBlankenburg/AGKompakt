namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityRequestDataFiles",
                c => new
                    {
                        ActivityRequestDataFileID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Data = c.Binary(),
                        ActivityRequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestDataFileID)
                .ForeignKey("dbo.ActivityRequests", t => t.ActivityRequestId, cascadeDelete: true)
                .Index(t => t.ActivityRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityRequestDataFiles", "ActivityRequestId", "dbo.ActivityRequests");
            DropIndex("dbo.ActivityRequestDataFiles", new[] { "ActivityRequestId" });
            DropTable("dbo.ActivityRequestDataFiles");
        }
    }
}
