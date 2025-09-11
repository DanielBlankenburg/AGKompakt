namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EMailDokstelle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgrammSettings", "EMailDokstelle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProgrammSettings", "EMailDokstelle");
        }
    }
}
