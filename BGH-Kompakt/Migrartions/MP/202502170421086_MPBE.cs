namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MPBE : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPBE",
                c => new
                    {
                        MPBEID = c.Int(nullable: false, identity: true),
                        MPBEName = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MPBEID);
            
            AddColumn("dbo.MPDecisions", "BE_MPBEID", c => c.Int());
            CreateIndex("dbo.MPDecisions", "BE_MPBEID");
            AddForeignKey("dbo.MPDecisions", "BE_MPBEID", "dbo.MPBE", "MPBEID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MPDecisions", "BE_MPBEID", "dbo.MPBE");
            DropIndex("dbo.MPDecisions", new[] { "BE_MPBEID" });
            DropColumn("dbo.MPDecisions", "BE_MPBEID");
            DropTable("dbo.MPBE");
        }
    }
}
