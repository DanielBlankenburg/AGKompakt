namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasOptionalARClient : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityRequests", "ActivityClientID", "dbo.ActivityClients");
            DropIndex("dbo.ActivityRequests", new[] { "ActivityClientID" });
            AlterColumn("dbo.ActivityRequests", "ActivityClientID", c => c.Int());
            CreateIndex("dbo.ActivityRequests", "ActivityClientID");
            AddForeignKey("dbo.ActivityRequests", "ActivityClientID", "dbo.ActivityClients", "ActivityClientId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityRequests", "ActivityClientID", "dbo.ActivityClients");
            DropIndex("dbo.ActivityRequests", new[] { "ActivityClientID" });
            AlterColumn("dbo.ActivityRequests", "ActivityClientID", c => c.Int(nullable: false));
            CreateIndex("dbo.ActivityRequests", "ActivityClientID");
            AddForeignKey("dbo.ActivityRequests", "ActivityClientID", "dbo.ActivityClients", "ActivityClientId", cascadeDelete: true);
        }
    }
}
