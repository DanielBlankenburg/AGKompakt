namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BSCWServer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MPSettings", "UploadBSCWServer", c => c.Boolean(nullable: false));
            AddColumn("dbo.MPSettings", "BSCWServerDrive", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MPSettings", "BSCWServerDrive");
            DropColumn("dbo.MPSettings", "UploadBSCWServer");
        }
    }
}
