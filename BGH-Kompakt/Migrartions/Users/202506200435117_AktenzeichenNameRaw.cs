namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AktenzeichenNameRaw : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SenatAktenzeichen", "SenatAktenzeichenNameRaw", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SenatAktenzeichen", "SenatAktenzeichenNameRaw");
        }
    }
}
