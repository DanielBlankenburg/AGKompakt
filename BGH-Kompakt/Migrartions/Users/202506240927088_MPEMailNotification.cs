namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MPEMailNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "MPEMailNotification", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "MPEMailNotification");
        }
    }
}
