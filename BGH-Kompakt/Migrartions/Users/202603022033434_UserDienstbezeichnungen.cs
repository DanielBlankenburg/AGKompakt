namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDienstbezeichnungen : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDienstbezeichnungen",
                c => new
                    {
                        UserDienstbezeichnungId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DienstbezeichnungId = c.Int(nullable: false),
                        GültigAb = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserDienstbezeichnungId)
                .ForeignKey("dbo.Dienstbezeichnungen", t => t.DienstbezeichnungId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.DienstbezeichnungId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDienstbezeichnungen", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserDienstbezeichnungen", "DienstbezeichnungId", "dbo.Dienstbezeichnungen");
            DropIndex("dbo.UserDienstbezeichnungen", new[] { "DienstbezeichnungId" });
            DropIndex("dbo.UserDienstbezeichnungen", new[] { "UserId" });
            DropTable("dbo.UserDienstbezeichnungen");
        }
    }
}
