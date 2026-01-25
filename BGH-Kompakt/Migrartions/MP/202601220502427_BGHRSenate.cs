namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BGHRSenate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPBGHRSenate",
                c => new
                    {
                        MPBGHRSenatID = c.Int(nullable: false, identity: true),
                        Senat = c.Int(nullable: false),
                        Recipient = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MPBGHRSenatID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MPBGHRSenate");
        }
    }
}
