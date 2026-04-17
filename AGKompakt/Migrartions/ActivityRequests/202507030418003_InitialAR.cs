namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialAR : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityClients",
                c => new
                    {
                        ActivityClientId = c.Int(nullable: false, identity: true),
                        ACName = c.String(nullable: false),
                        ActivityRequestId = c.Int(nullable: false),
                        ActivityClientTypID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityClientId)
                .ForeignKey("dbo.ActivityClientTyps", t => t.ActivityClientTypID, cascadeDelete: true)
                .Index(t => t.ActivityClientTypID);
            
            CreateTable(
                "dbo.ActivityClientTyps",
                c => new
                    {
                        ActivityClientTypId = c.Int(nullable: false, identity: true),
                        ActivityClientTypText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityClientTypId);
            
            CreateTable(
                "dbo.ActivityRequests",
                c => new
                    {
                        ActivityRequestId = c.Int(nullable: false, identity: true),
                        ARDatum = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ARTitel = c.String(nullable: false, maxLength: 255),
                        ARVerguetung = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ARZeitaufwandMain = c.Single(nullable: false),
                        ARZeitaufwandPrep = c.Single(),
                        ARNote = c.String(),
                        AROrt = c.String(),
                        ARActivityDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ARAttechments = c.Boolean(),
                        ActivityRequestDatePermanentFrom = c.DateTime(precision: 7, storeType: "datetime2"),
                        ActivityRequestDatePermanentUntil = c.DateTime(precision: 7, storeType: "datetime2"),
                        ActivityRequestDatePermantenDuration = c.Int(nullable: false),
                        ActivityRequestArbitrationCaller = c.String(),
                        ARUserID = c.Int(nullable: false),
                        ActivityRequestTypID = c.Int(),
                        ActivityRequestMeldeArtID = c.Int(nullable: false),
                        ActivityClientID = c.Int(nullable: false),
                        ActivityRequestScienceTypId = c.Int(),
                        ActivityRequestScienceCategorieId = c.Int(),
                        ActivityRequestScienceAuthorId = c.Int(),
                        ActivityRequestOrtArtId = c.Int(),
                        ActivityRequestFrequencyId = c.Int(),
                        ActivityRequestArbitrationTypId = c.Int(),
                        ARZustaendigkeitsbereich = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestId)
                .ForeignKey("dbo.ActivityClients", t => t.ActivityClientID, cascadeDelete: true)
                .ForeignKey("dbo.ActivityRequestArbitrationTyps", t => t.ActivityRequestArbitrationTypId)
                .ForeignKey("dbo.ActivityRequestFrequencies", t => t.ActivityRequestFrequencyId)
                .ForeignKey("dbo.ActivityRequestMeldeArten", t => t.ActivityRequestMeldeArtID, cascadeDelete: true)
                .ForeignKey("dbo.ActivityRequestOrtArten", t => t.ActivityRequestOrtArtId)
                .ForeignKey("dbo.ActivityRequestScienceAuthors", t => t.ActivityRequestScienceAuthorId)
                .ForeignKey("dbo.ActivityRequestScienceCategories", t => t.ActivityRequestScienceCategorieId)
                .ForeignKey("dbo.ActivityRequestScienceTyps", t => t.ActivityRequestScienceTypId)
                .ForeignKey("dbo.ActivityRequestTyps", t => t.ActivityRequestTypID)
                .Index(t => t.ActivityRequestTypID)
                .Index(t => t.ActivityRequestMeldeArtID)
                .Index(t => t.ActivityClientID)
                .Index(t => t.ActivityRequestScienceTypId)
                .Index(t => t.ActivityRequestScienceCategorieId)
                .Index(t => t.ActivityRequestScienceAuthorId)
                .Index(t => t.ActivityRequestOrtArtId)
                .Index(t => t.ActivityRequestFrequencyId)
                .Index(t => t.ActivityRequestArbitrationTypId);
            
            CreateTable(
                "dbo.ActivityRequestArbitrationClients",
                c => new
                    {
                        ActivityRequestArbitrationClientId = c.Int(nullable: false, identity: true),
                        ActivityRequestArbitrationClientText = c.String(nullable: false),
                        ActivityRequest_ActivityRequestId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityRequestArbitrationClientId)
                .ForeignKey("dbo.ActivityRequests", t => t.ActivityRequest_ActivityRequestId)
                .Index(t => t.ActivityRequest_ActivityRequestId);
            
            CreateTable(
                "dbo.ActivityRequestArbitrationTyps",
                c => new
                    {
                        ActivityRequestArbitrationTypId = c.Int(nullable: false, identity: true),
                        ActivityRequestArbitrationTypText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestArbitrationTypId);
            
            CreateTable(
                "dbo.ActivityRequestFrequencies",
                c => new
                    {
                        ActivityRequestFrequencyId = c.Int(nullable: false, identity: true),
                        ActivityRequestFrequencyText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestFrequencyId);
            
            CreateTable(
                "dbo.ActivityRequestMeldeArten",
                c => new
                    {
                        ActivityRequestMeldeArtId = c.Int(nullable: false, identity: true),
                        ActivityRequestMeldeArtText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestMeldeArtId);
            
            CreateTable(
                "dbo.ActivityRequestOrtArten",
                c => new
                    {
                        ActivityRequestOrtArtId = c.Int(nullable: false, identity: true),
                        ActivityRequestOrtArtText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestOrtArtId);
            
            CreateTable(
                "dbo.ActivityRequestScienceAuthors",
                c => new
                    {
                        ActivityRequestScienceAuthorId = c.Int(nullable: false, identity: true),
                        ActivityRequestScienceAuthorText = c.String(nullable: false),
                        IsSelected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestScienceAuthorId);
            
            CreateTable(
                "dbo.ActivityRequestScienceCategories",
                c => new
                    {
                        ActivityRequestScienceCategorieId = c.Int(nullable: false, identity: true),
                        ActivityRequestScienceCategorieText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestScienceCategorieId);
            
            CreateTable(
                "dbo.ActivityRequestScienceTyps",
                c => new
                    {
                        ActivityRequestScienceTypId = c.Int(nullable: false, identity: true),
                        ActivityRequestScienceTypText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestScienceTypId);
            
            CreateTable(
                "dbo.ActivityRequestTyps",
                c => new
                    {
                        ActivityRequestTypId = c.Int(nullable: false, identity: true),
                        ActivityRequestTypText = c.String(nullable: false),
                        ActivityRequestTypMeldeArt = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestTypId);
            
            CreateTable(
                "dbo.ARVerguetungAdventages",
                c => new
                    {
                        ARVerguetungAdventageId = c.Int(nullable: false, identity: true),
                        ARVerguetungAdventageTypId = c.Int(nullable: false),
                        ARVerguetungAdventageAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActivityRequest_ActivityRequestId = c.Int(),
                    })
                .PrimaryKey(t => t.ARVerguetungAdventageId)
                .ForeignKey("dbo.ActivityRequests", t => t.ActivityRequest_ActivityRequestId)
                .ForeignKey("dbo.ARVerguetungAdventageTyps", t => t.ARVerguetungAdventageTypId, cascadeDelete: true)
                .Index(t => t.ARVerguetungAdventageTypId)
                .Index(t => t.ActivityRequest_ActivityRequestId);
            
            CreateTable(
                "dbo.ARVerguetungAdventageTyps",
                c => new
                    {
                        ARVerguetungAdventageTypId = c.Int(nullable: false, identity: true),
                        ARVerguetungAdventageTypText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ARVerguetungAdventageTypId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ARVerguetungAdventages", "ARVerguetungAdventageTypId", "dbo.ARVerguetungAdventageTyps");
            DropForeignKey("dbo.ARVerguetungAdventages", "ActivityRequest_ActivityRequestId", "dbo.ActivityRequests");
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestTypID", "dbo.ActivityRequestTyps");
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestScienceTypId", "dbo.ActivityRequestScienceTyps");
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestScienceCategorieId", "dbo.ActivityRequestScienceCategories");
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestScienceAuthorId", "dbo.ActivityRequestScienceAuthors");
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestOrtArtId", "dbo.ActivityRequestOrtArten");
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestMeldeArtID", "dbo.ActivityRequestMeldeArten");
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestFrequencyId", "dbo.ActivityRequestFrequencies");
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestArbitrationTypId", "dbo.ActivityRequestArbitrationTyps");
            DropForeignKey("dbo.ActivityRequestArbitrationClients", "ActivityRequest_ActivityRequestId", "dbo.ActivityRequests");
            DropForeignKey("dbo.ActivityRequests", "ActivityClientID", "dbo.ActivityClients");
            DropForeignKey("dbo.ActivityClients", "ActivityClientTypID", "dbo.ActivityClientTyps");
            DropIndex("dbo.ARVerguetungAdventages", new[] { "ActivityRequest_ActivityRequestId" });
            DropIndex("dbo.ARVerguetungAdventages", new[] { "ARVerguetungAdventageTypId" });
            DropIndex("dbo.ActivityRequestArbitrationClients", new[] { "ActivityRequest_ActivityRequestId" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestArbitrationTypId" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestFrequencyId" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestOrtArtId" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestScienceAuthorId" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestScienceCategorieId" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestScienceTypId" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityClientID" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestMeldeArtID" });
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestTypID" });
            DropIndex("dbo.ActivityClients", new[] { "ActivityClientTypID" });
            DropTable("dbo.ARVerguetungAdventageTyps");
            DropTable("dbo.ARVerguetungAdventages");
            DropTable("dbo.ActivityRequestTyps");
            DropTable("dbo.ActivityRequestScienceTyps");
            DropTable("dbo.ActivityRequestScienceCategories");
            DropTable("dbo.ActivityRequestScienceAuthors");
            DropTable("dbo.ActivityRequestOrtArten");
            DropTable("dbo.ActivityRequestMeldeArten");
            DropTable("dbo.ActivityRequestFrequencies");
            DropTable("dbo.ActivityRequestArbitrationTyps");
            DropTable("dbo.ActivityRequestArbitrationClients");
            DropTable("dbo.ActivityRequests");
            DropTable("dbo.ActivityClientTyps");
            DropTable("dbo.ActivityClients");
        }
    }
}
