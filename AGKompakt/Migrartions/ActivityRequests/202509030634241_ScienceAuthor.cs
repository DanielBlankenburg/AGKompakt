namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScienceAuthor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityRequests", "ActivityRequestScienceAuthorId", "dbo.ActivityRequestScienceAuthors");
            DropIndex("dbo.ActivityRequests", new[] { "ActivityRequestScienceAuthorId" });
            CreateTable(
                "dbo.ActivityRequestScienceAuthorNames",
                c => new
                    {
                        ActivityRequestScienceAuthorNameId = c.Int(nullable: false, identity: true),
                        ActivityRequestScienceAuthorText = c.String(nullable: false),
                        IsSelected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestScienceAuthorNameId);
            
            AddColumn("dbo.ActivityRequests", "SciencenAuthorAuthor", c => c.Boolean(nullable: false));
            AddColumn("dbo.ActivityRequests", "SciencenAuthorSchriftleitung", c => c.Boolean(nullable: false));
            AddColumn("dbo.ActivityRequests", "SciencenAuthorHerausgeber", c => c.Boolean(nullable: false));
            AddColumn("dbo.ActivityRequests", "SciencenAuthorWissenschaftlicherBeirat", c => c.Boolean(nullable: false));
            AddColumn("dbo.ActivityRequests", "SciencenAuthorSonstiges", c => c.Boolean(nullable: false));
            DropColumn("dbo.ActivityRequests", "ActivityRequestScienceAuthorId");
            DropTable("dbo.ActivityRequestScienceAuthors");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ActivityRequestScienceAuthors",
                c => new
                    {
                        ActivityRequestScienceAuthorId = c.Int(nullable: false, identity: true),
                        ActivityRequestScienceAuthorText = c.String(nullable: false),
                        IsSelected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityRequestScienceAuthorId);
            
            AddColumn("dbo.ActivityRequests", "ActivityRequestScienceAuthorId", c => c.Int());
            DropColumn("dbo.ActivityRequests", "SciencenAuthorSonstiges");
            DropColumn("dbo.ActivityRequests", "SciencenAuthorWissenschaftlicherBeirat");
            DropColumn("dbo.ActivityRequests", "SciencenAuthorHerausgeber");
            DropColumn("dbo.ActivityRequests", "SciencenAuthorSchriftleitung");
            DropColumn("dbo.ActivityRequests", "SciencenAuthorAuthor");
            DropTable("dbo.ActivityRequestScienceAuthorNames");
            CreateIndex("dbo.ActivityRequests", "ActivityRequestScienceAuthorId");
            AddForeignKey("dbo.ActivityRequests", "ActivityRequestScienceAuthorId", "dbo.ActivityRequestScienceAuthors", "ActivityRequestScienceAuthorId");
        }
    }
}
