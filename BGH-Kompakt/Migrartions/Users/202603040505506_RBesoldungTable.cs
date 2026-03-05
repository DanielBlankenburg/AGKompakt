namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RBesoldungTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RBesoldungen",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        paymentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DienstbezeichnungenID = c.Int(nullable: false),
                        Dienstbezeichnung_DienstbezeichnungId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Dienstbezeichnungen", t => t.Dienstbezeichnung_DienstbezeichnungId, cascadeDelete: true)
                .Index(t => t.Dienstbezeichnung_DienstbezeichnungId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RBesoldungen", "Dienstbezeichnung_DienstbezeichnungId", "dbo.Dienstbezeichnungen");
            DropIndex("dbo.RBesoldungen", new[] { "Dienstbezeichnung_DienstbezeichnungId" });
            DropTable("dbo.RBesoldungen");
        }
    }
}
