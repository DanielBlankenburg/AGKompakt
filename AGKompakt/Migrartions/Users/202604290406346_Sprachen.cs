namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sprachen : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sprachen",
                c => new
                    {
                        SpracheID = c.Int(nullable: false, identity: true),
                        SpracheText = c.String(),
                    })
                .PrimaryKey(t => t.SpracheID);
            
            CreateTable(
                "dbo.Verfahrensbeistand_Sprache",
                c => new
                    {
                        VerfahrensbeistandId = c.Int(nullable: false),
                        SpracheId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VerfahrensbeistandId, t.SpracheId })
                .ForeignKey("dbo.Verfahrensbeistaende", t => t.VerfahrensbeistandId, cascadeDelete: true)
                .ForeignKey("dbo.Sprachen", t => t.SpracheId, cascadeDelete: true)
                .Index(t => t.VerfahrensbeistandId)
                .Index(t => t.SpracheId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Verfahrensbeistand_Sprache", "SpracheId", "dbo.Sprachen");
            DropForeignKey("dbo.Verfahrensbeistand_Sprache", "VerfahrensbeistandId", "dbo.Verfahrensbeistaende");
            DropIndex("dbo.Verfahrensbeistand_Sprache", new[] { "SpracheId" });
            DropIndex("dbo.Verfahrensbeistand_Sprache", new[] { "VerfahrensbeistandId" });
            DropTable("dbo.Verfahrensbeistand_Sprache");
            DropTable("dbo.Sprachen");
        }
    }
}
