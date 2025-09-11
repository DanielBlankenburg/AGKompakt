namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vermerke : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPVermerke",
                c => new
                    {
                        MPVermerkID = c.Int(nullable: false, identity: true),
                        MPVermerkText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MPVermerkID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MPVermerke");
        }
    }
}
