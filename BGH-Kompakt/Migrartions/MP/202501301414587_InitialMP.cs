namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPCategories",
                c => new
                    {
                        MPCategoryID = c.Int(nullable: false, identity: true),
                        MPCategoryText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MPCategoryID);
            
            CreateTable(
                "dbo.MPSenate",
                c => new
                    {
                        MPSenatID = c.Int(nullable: false, identity: true),
                        MPSenatName = c.String(nullable: false),
                        MPCategorieID = c.Int(nullable: false),
                        MPSenatSorting = c.Int(nullable: false),
                        MPDecisionID = c.Int(),
                    })
                .PrimaryKey(t => t.MPSenatID)
                .ForeignKey("dbo.MPCategories", t => t.MPCategorieID, cascadeDelete: true)
                .Index(t => t.MPCategorieID);
            
            CreateTable(
                "dbo.MPDecisions",
                c => new
                    {
                        MPDecisionID = c.Int(nullable: false, identity: true),
                        Rohdaten = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Typ = c.Int(nullable: false),
                        Rechtsgebiet = c.String(),
                        Normenkette = c.String(),
                        Leitsatz = c.String(),
                        Aktenzeichen = c.String(nullable: false),
                        SenatID = c.Int(nullable: false),
                        RegZeichen = c.String(nullable: false),
                        LaufendeNummer = c.String(nullable: false),
                        Jahr = c.String(nullable: false),
                        InstanzErste = c.String(),
                        InstanzZweite = c.String(),
                        MPWeekID = c.Int(nullable: false),
                        PathName = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MPDecisionID)
                .ForeignKey("dbo.MPWeeks", t => t.MPWeekID, cascadeDelete: true)
                .ForeignKey("dbo.MPSenate", t => t.SenatID, cascadeDelete: true)
                .Index(t => t.SenatID)
                .Index(t => t.MPWeekID);
            
            CreateTable(
                "dbo.MPWeeks",
                c => new
                    {
                        MPWeekID = c.Int(nullable: false, identity: true),
                        MPWeekYear = c.Int(nullable: false),
                        MPWeekNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MPWeekID);
            
            CreateTable(
                "dbo.MPSenatAbbreviations",
                c => new
                    {
                        MPSenatAbbreviationID = c.Int(nullable: false, identity: true),
                        MPSenatAbbreviationText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MPSenatAbbreviationID);
            
            CreateTable(
                "dbo.MPSenatMPSenatAbbreviations",
                c => new
                    {
                        MPSenat_MPSenatID = c.Int(nullable: false),
                        MPSenatAbbreviation_MPSenatAbbreviationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MPSenat_MPSenatID, t.MPSenatAbbreviation_MPSenatAbbreviationID })
                .ForeignKey("dbo.MPSenate", t => t.MPSenat_MPSenatID, cascadeDelete: true)
                .ForeignKey("dbo.MPSenatAbbreviations", t => t.MPSenatAbbreviation_MPSenatAbbreviationID, cascadeDelete: true)
                .Index(t => t.MPSenat_MPSenatID)
                .Index(t => t.MPSenatAbbreviation_MPSenatAbbreviationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MPSenatMPSenatAbbreviations", "MPSenatAbbreviation_MPSenatAbbreviationID", "dbo.MPSenatAbbreviations");
            DropForeignKey("dbo.MPSenatMPSenatAbbreviations", "MPSenat_MPSenatID", "dbo.MPSenate");
            DropForeignKey("dbo.MPDecisions", "SenatID", "dbo.MPSenate");
            DropForeignKey("dbo.MPDecisions", "MPWeekID", "dbo.MPWeeks");
            DropForeignKey("dbo.MPSenate", "MPCategorieID", "dbo.MPCategories");
            DropIndex("dbo.MPSenatMPSenatAbbreviations", new[] { "MPSenatAbbreviation_MPSenatAbbreviationID" });
            DropIndex("dbo.MPSenatMPSenatAbbreviations", new[] { "MPSenat_MPSenatID" });
            DropIndex("dbo.MPDecisions", new[] { "MPWeekID" });
            DropIndex("dbo.MPDecisions", new[] { "SenatID" });
            DropIndex("dbo.MPSenate", new[] { "MPCategorieID" });
            DropTable("dbo.MPSenatMPSenatAbbreviations");
            DropTable("dbo.MPSenatAbbreviations");
            DropTable("dbo.MPWeeks");
            DropTable("dbo.MPDecisions");
            DropTable("dbo.MPSenate");
            DropTable("dbo.MPCategories");
        }
    }
}
