namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_017_2_Besoldung : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Besoldungsgruppen",
                c => new
                    {
                        BesoldungsgruppeId = c.Int(nullable: false, identity: true),
                        BesoldungsgruppeText = c.String(),
                    })
                .PrimaryKey(t => t.BesoldungsgruppeId);
            
            AddColumn("dbo.Dienstbezeichnungen", "Besoldungsgruppe_BesoldungsgruppeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Dienstbezeichnungen", "Besoldungsgruppe_BesoldungsgruppeId");
            AddForeignKey("dbo.Dienstbezeichnungen", "Besoldungsgruppe_BesoldungsgruppeId", "dbo.Besoldungsgruppen", "BesoldungsgruppeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dienstbezeichnungen", "Besoldungsgruppe_BesoldungsgruppeId", "dbo.Besoldungsgruppen");
            DropIndex("dbo.Dienstbezeichnungen", new[] { "Besoldungsgruppe_BesoldungsgruppeId" });
            DropColumn("dbo.Dienstbezeichnungen", "Besoldungsgruppe_BesoldungsgruppeId");
            DropTable("dbo.Besoldungsgruppen");
        }
    }
}
