namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialVerfahrensbeistaende : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Verfahrensbeistaende",
                c => new
                    {
                        VerfahrensbeistandId = c.Int(nullable: false, identity: true),
                        VorName = c.String(nullable: false),
                        NachName = c.String(nullable: false),
                        EMail = c.String(nullable: false),
                        GeschlechtID = c.Int(nullable: false),
                        TitelId = c.Int(),
                    })
                .PrimaryKey(t => t.VerfahrensbeistandId)
                .ForeignKey("dbo.Geschlechter", t => t.GeschlechtID, cascadeDelete: true)
                .ForeignKey("dbo.Titels", t => t.TitelId)
                .Index(t => t.GeschlechtID)
                .Index(t => t.TitelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Verfahrensbeistaende", "TitelId", "dbo.Titels");
            DropForeignKey("dbo.Verfahrensbeistaende", "GeschlechtID", "dbo.Geschlechter");
            DropIndex("dbo.Verfahrensbeistaende", new[] { "TitelId" });
            DropIndex("dbo.Verfahrensbeistaende", new[] { "GeschlechtID" });
            DropTable("dbo.Verfahrensbeistaende");
        }
    }
}
