namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialPayments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RBesoldungen",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.RBesoldungPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        paymentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RBesoldungId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RBesoldungen", t => t.RBesoldungId, cascadeDelete: true)
                .Index(t => t.RBesoldungId);
            
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
            
            AddColumn("dbo.Dienstbezeichnungen", "BesoldungsgruppeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Dienstbezeichnungen", "BesoldungsgruppeID");
            AddForeignKey("dbo.Dienstbezeichnungen", "BesoldungsgruppeID", "dbo.RBesoldungen", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDienstbezeichnungen", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserDienstbezeichnungen", "DienstbezeichnungId", "dbo.Dienstbezeichnungen");
            DropForeignKey("dbo.Dienstbezeichnungen", "BesoldungsgruppeID", "dbo.RBesoldungen");
            DropForeignKey("dbo.RBesoldungPayments", "RBesoldungId", "dbo.RBesoldungen");
            DropIndex("dbo.UserDienstbezeichnungen", new[] { "DienstbezeichnungId" });
            DropIndex("dbo.UserDienstbezeichnungen", new[] { "UserId" });
            DropIndex("dbo.RBesoldungPayments", new[] { "RBesoldungId" });
            DropIndex("dbo.Dienstbezeichnungen", new[] { "BesoldungsgruppeID" });
            DropColumn("dbo.Dienstbezeichnungen", "BesoldungsgruppeID");
            DropTable("dbo.UserDienstbezeichnungen");
            DropTable("dbo.RBesoldungPayments");
            DropTable("dbo.RBesoldungen");
        }
    }
}
