namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BSCWSubFolders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "MPBSCWSubFolders", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "MPBSCWSubFolders");
        }
    }
}
