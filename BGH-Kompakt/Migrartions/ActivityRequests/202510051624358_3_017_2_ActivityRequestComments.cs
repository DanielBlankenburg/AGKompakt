namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_017_2_ActivityRequestComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityRequestComments",
                c => new
                    {
                        ActivityRequestCommentID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        ActivityRequestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestCommentID)
                .ForeignKey("dbo.ActivityRequests", t => t.ActivityRequestID, cascadeDelete: true)
                .Index(t => t.ActivityRequestID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityRequestComments", "ActivityRequestID", "dbo.ActivityRequests");
            DropIndex("dbo.ActivityRequestComments", new[] { "ActivityRequestID" });
            DropTable("dbo.ActivityRequestComments");
        }
    }
}
