namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPayments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RBesoldungen", "Dienstbezeichnung_DienstbezeichnungId", "dbo.Dienstbezeichnungen");
            DropIndex("dbo.RBesoldungen", new[] { "Dienstbezeichnung_DienstbezeichnungId" });
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
            
            AddColumn("dbo.Dienstbezeichnungen", "BesoldungsgruppeID", c => c.Int(nullable: false));
            AddColumn("dbo.RBesoldungen", "Name", c => c.String());
            CreateIndex("dbo.Dienstbezeichnungen", "BesoldungsgruppeID");
            AddForeignKey("dbo.Dienstbezeichnungen", "BesoldungsgruppeID", "dbo.RBesoldungen", "id", cascadeDelete: true);
            DropColumn("dbo.RBesoldungen", "Start");
            DropColumn("dbo.RBesoldungen", "paymentValue");
            DropColumn("dbo.RBesoldungen", "DienstbezeichnungenID");
            DropColumn("dbo.RBesoldungen", "Dienstbezeichnung_DienstbezeichnungId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RBesoldungen", "Dienstbezeichnung_DienstbezeichnungId", c => c.Int(nullable: false));
            AddColumn("dbo.RBesoldungen", "DienstbezeichnungenID", c => c.Int(nullable: false));
            AddColumn("dbo.RBesoldungen", "paymentValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.RBesoldungen", "Start", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Dienstbezeichnungen", "BesoldungsgruppeID", "dbo.RBesoldungen");
            DropForeignKey("dbo.RBesoldungPayments", "RBesoldungId", "dbo.RBesoldungen");
            DropIndex("dbo.RBesoldungPayments", new[] { "RBesoldungId" });
            DropIndex("dbo.Dienstbezeichnungen", new[] { "BesoldungsgruppeID" });
            DropColumn("dbo.RBesoldungen", "Name");
            DropColumn("dbo.Dienstbezeichnungen", "BesoldungsgruppeID");
            DropTable("dbo.RBesoldungPayments");
            CreateIndex("dbo.RBesoldungen", "Dienstbezeichnung_DienstbezeichnungId");
            AddForeignKey("dbo.RBesoldungen", "Dienstbezeichnung_DienstbezeichnungId", "dbo.Dienstbezeichnungen", "DienstbezeichnungId", cascadeDelete: true);
        }
    }
}
