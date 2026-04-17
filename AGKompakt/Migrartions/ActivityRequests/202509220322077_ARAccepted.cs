namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARAccepted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityRequests", "ActivityRequestAccepted", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActivityRequests", "ActivityRequestAccepted");
        }
    }
}
