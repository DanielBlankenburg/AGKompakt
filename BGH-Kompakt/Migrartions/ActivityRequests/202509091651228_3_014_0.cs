namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_014_0 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestVerguetungTypId", "dbo.ARVerguetungTyps");
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestVerguetungTypId" });
            AddColumn("dbo.ActivityRequests", "ActivityRequestHourTypId", c => c.Int(nullable: false));
            DropTable("dbo.ARVerguetungTyps");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ARVerguetungTyps",
                c => new
                    {
                        ARVerguetungTypId = c.Int(nullable: false, identity: true),
                        ARVerguetungTypText = c.String(),
                    })
                .PrimaryKey(t => t.ARVerguetungTypId);
            
            DropColumn("dbo.ActivityRequests", "ActivityRequestHourTypId");
            CreateIndex("dbo.ActivityRequests", "ActivityRequestVerguetungTypId");
            AddForeignKey("dbo.ActivityRequests", "ActivityRequestVerguetungTypId", "dbo.ARVerguetungTyps", "ARVerguetungTypId");
        }
    }
}
