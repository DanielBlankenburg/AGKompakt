namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_016_Straf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SenatSettings", "StrafFolderSenat", c => c.Boolean(nullable: false));
            AddColumn("dbo.SenatSettings", "StrafFolderSubFolder", c => c.Boolean(nullable: false));
            AddColumn("dbo.SenatSettings", "StrafFolderSubFolderText", c => c.String());
            AddColumn("dbo.SenatSettings", "StrafFolderBerichterstatter", c => c.Boolean(nullable: false));
            AddColumn("dbo.SenatSettings", "StrafFolderYearFirst", c => c.Boolean(nullable: false));
            AddColumn("dbo.SenatSettings", "StrafFileAzPrefix", c => c.Boolean(nullable: false));
            AddColumn("dbo.SenatSettings", "StrafFileSenat", c => c.Boolean(nullable: false));
            AddColumn("dbo.SenatSettings", "StrafFileSenatsheftText", c => c.String());
            AddColumn("dbo.SenatSettings", "StrafFileWhiteSpaceFill", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SenatSettings", "StrafFileWhiteSpaceFill");
            DropColumn("dbo.SenatSettings", "StrafFileSenatsheftText");
            DropColumn("dbo.SenatSettings", "StrafFileSenat");
            DropColumn("dbo.SenatSettings", "StrafFileAzPrefix");
            DropColumn("dbo.SenatSettings", "StrafFolderYearFirst");
            DropColumn("dbo.SenatSettings", "StrafFolderBerichterstatter");
            DropColumn("dbo.SenatSettings", "StrafFolderSubFolderText");
            DropColumn("dbo.SenatSettings", "StrafFolderSubFolder");
            DropColumn("dbo.SenatSettings", "StrafFolderSenat");
        }
    }
}
