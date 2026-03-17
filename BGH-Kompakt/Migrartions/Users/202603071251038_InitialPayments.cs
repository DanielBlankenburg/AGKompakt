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
                        PaymentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RBesoldung_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RBesoldungen", t => t.RBesoldung_id, cascadeDelete: true)
                .Index(t => t.RBesoldung_id);
            
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
            
            AddColumn("dbo.Dienstbezeichnungen", "Besoldungsgruppe_id", c => c.Int());
            CreateIndex("dbo.Dienstbezeichnungen", "Besoldungsgruppe_id");
            AddForeignKey("dbo.Dienstbezeichnungen", "Besoldungsgruppe_id", "dbo.RBesoldungen", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDienstbezeichnungen", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserDienstbezeichnungen", "DienstbezeichnungId", "dbo.Dienstbezeichnungen");
            DropForeignKey("dbo.Dienstbezeichnungen", "Besoldungsgruppe_id", "dbo.RBesoldungen");
            DropForeignKey("dbo.RBesoldungPayments", "RBesoldung_id", "dbo.RBesoldungen");
            DropIndex("dbo.UserDienstbezeichnungen", new[] { "DienstbezeichnungId" });
            DropIndex("dbo.UserDienstbezeichnungen", new[] { "UserId" });
            DropIndex("dbo.RBesoldungPayments", new[] { "RBesoldung_id" });
            DropIndex("dbo.Dienstbezeichnungen", new[] { "Besoldungsgruppe_id" });
            DropColumn("dbo.Dienstbezeichnungen", "Besoldungsgruppe_id");
            DropTable("dbo.UserDienstbezeichnungen");
            DropTable("dbo.RBesoldungPayments");
            DropTable("dbo.RBesoldungen");
        }
    }
}
