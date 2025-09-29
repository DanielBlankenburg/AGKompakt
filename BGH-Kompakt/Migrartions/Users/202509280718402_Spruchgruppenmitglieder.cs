namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Spruchgruppenmitglieder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "SenatSpruchgruppe_SenatSpruchgruppeID", "dbo.SenatSpruchgruppen");
            DropIndex("dbo.Users", new[] { "SenatSpruchgruppe_SenatSpruchgruppeID" });
            CreateTable(
                "dbo.Spruchgruppenmitglieder",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        SenatSpruchgruppe_SenatSpruchgruppeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.SenatSpruchgruppe_SenatSpruchgruppeID })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.SenatSpruchgruppen", t => t.SenatSpruchgruppe_SenatSpruchgruppeID, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.SenatSpruchgruppe_SenatSpruchgruppeID);
            
            DropColumn("dbo.Users", "SenatSpruchgruppe_SenatSpruchgruppeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "SenatSpruchgruppe_SenatSpruchgruppeID", c => c.Int());
            DropForeignKey("dbo.Spruchgruppenmitglieder", "SenatSpruchgruppe_SenatSpruchgruppeID", "dbo.SenatSpruchgruppen");
            DropForeignKey("dbo.Spruchgruppenmitglieder", "User_UserId", "dbo.Users");
            DropIndex("dbo.Spruchgruppenmitglieder", new[] { "SenatSpruchgruppe_SenatSpruchgruppeID" });
            DropIndex("dbo.Spruchgruppenmitglieder", new[] { "User_UserId" });
            DropTable("dbo.Spruchgruppenmitglieder");
            CreateIndex("dbo.Users", "SenatSpruchgruppe_SenatSpruchgruppeID");
            AddForeignKey("dbo.Users", "SenatSpruchgruppe_SenatSpruchgruppeID", "dbo.SenatSpruchgruppen", "SenatSpruchgruppeID");
        }
    }
}
