namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_017_2_BE : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MPDecisions", "BE_MPBEID", "dbo.MPBE");
            DropIndex("dbo.MPDecisions", new[] { "BE_MPBEID" });
            AddColumn("dbo.MPDecisions", "BE", c => c.Int(nullable: false));
            DropColumn("dbo.MPDecisions", "BE_MPBEID");
            DropTable("dbo.MPBE");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MPBE",
                c => new
                    {
                        MPBEID = c.Int(nullable: false, identity: true),
                        MPBEName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MPBEID);
            
            AddColumn("dbo.MPDecisions", "BE_MPBEID", c => c.Int());
            DropColumn("dbo.MPDecisions", "BE");
            CreateIndex("dbo.MPDecisions", "BE_MPBEID");
            AddForeignKey("dbo.MPDecisions", "BE_MPBEID", "dbo.MPBE", "MPBEID");
        }
    }
}
