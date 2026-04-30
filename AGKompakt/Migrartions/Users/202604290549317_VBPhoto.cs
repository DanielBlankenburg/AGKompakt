namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VBPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Verfahrensbeistaende", "Photo", c => c.Binary());
            AddColumn("dbo.Verfahrensbeistaende", "PhotoFileName", c => c.String());
            AddColumn("dbo.Verfahrensbeistaende", "PhotoContentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Verfahrensbeistaende", "PhotoContentType");
            DropColumn("dbo.Verfahrensbeistaende", "PhotoFileName");
            DropColumn("dbo.Verfahrensbeistaende", "Photo");
        }
    }
}
