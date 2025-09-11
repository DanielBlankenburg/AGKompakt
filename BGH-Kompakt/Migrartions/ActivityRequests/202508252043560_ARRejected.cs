namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARRejected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityRequests", "ARRejected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActivityRequests", "ARRejected");
        }
    }
}
