namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARVerguetungType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ARVerguetungTyps",
                c => new
                    {
                        ARVerguetungTypId = c.Int(nullable: false, identity: true),
                        ARVerguetungTypText = c.String(),
                    })
                .PrimaryKey(t => t.ARVerguetungTypId);
            
            AddColumn("dbo.ActivityRequests", "ActivityRequestVerguetungTypId", c => c.Int());
            CreateIndex("dbo.ActivityRequests", "ActivityRequestVerguetungTypId");
            AddForeignKey("dbo.ActivityRequests", "ActivityRequestVerguetungTypId", "dbo.ARVerguetungTyps", "ARVerguetungTypId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestVerguetungTypId", "dbo.ARVerguetungTyps");
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestVerguetungTypId" });
            DropColumn("dbo.ActivityRequests", "ActivityRequestVerguetungTypId");
            DropTable("dbo.ARVerguetungTyps");
        }
    }
}
