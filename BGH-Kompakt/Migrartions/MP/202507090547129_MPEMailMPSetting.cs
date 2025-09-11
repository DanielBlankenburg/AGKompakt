namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MPEMailMPSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPEMails",
                c => new
                    {
                        MPEMailID = c.Int(nullable: false, identity: true),
                        MPEMailDescription = c.String(nullable: false),
                        MPEMailBody = c.String(),
                        MPEMailSubject = c.String(),
                    })
                .PrimaryKey(t => t.MPEMailID);
            
            CreateTable(
                "dbo.MPSettings",
                c => new
                    {
                        MPSettingID = c.Int(nullable: false, identity: true),
                        MPSettingEMailAnrede = c.String(),
                        MPSettingEMailSchluss = c.String(),
                        MPSettingDatenschutzhinweis = c.String(),
                    })
                .PrimaryKey(t => t.MPSettingID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MPSettings");
            DropTable("dbo.MPEMails");
        }
    }
}
