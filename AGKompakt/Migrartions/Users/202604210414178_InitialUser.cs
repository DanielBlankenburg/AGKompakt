namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminStatus",
                c => new
                    {
                        AdminStatusID = c.Int(nullable: false, identity: true),
                        AdminStatusText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AdminStatusID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        VorName = c.String(nullable: false),
                        NachName = c.String(nullable: false),
                        EMail = c.String(nullable: false),
                        ComputerName = c.String(nullable: false),
                        GeschlechtID = c.Int(nullable: false),
                        TitelId = c.Int(),
                        PositionId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        DienstbezeichnungId = c.Int(),
                        MPBSCW_Server_Drive = c.Int(),
                        Initials = c.String(),
                        Testfield = c.String(),
                        Testzahl = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Dienstbezeichnungen", t => t.DienstbezeichnungId)
                .ForeignKey("dbo.Geschlechter", t => t.GeschlechtID, cascadeDelete: true)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.Titels", t => t.TitelId)
                .Index(t => t.GeschlechtID)
                .Index(t => t.TitelId)
                .Index(t => t.PositionId)
                .Index(t => t.StatusId)
                .Index(t => t.DienstbezeichnungId);
            
            CreateTable(
                "dbo.Dienstbezeichnungen",
                c => new
                    {
                        DienstbezeichnungId = c.Int(nullable: false, identity: true),
                        DienstbezeichnungText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DienstbezeichnungId);
            
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
            
            CreateTable(
                "dbo.Geschlechter",
                c => new
                    {
                        GeschlechtID = c.Int(nullable: false, identity: true),
                        GeschlechtText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GeschlechtID);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        PositionText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PositionId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        StatusText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dbo.Titels",
                c => new
                    {
                        TitelId = c.Int(nullable: false, identity: true),
                        TitelText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TitelId);
            
            CreateTable(
                "dbo.ProgrammSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PathAG = c.String(),
                        PathFam = c.String(),
                        PathZiv = c.String(),
                        PathInsO = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rollen",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        AdminStatus_AdminStatusID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.AdminStatus_AdminStatusID })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.AdminStatus", t => t.AdminStatus_AdminStatusID, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.AdminStatus_AdminStatusID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDienstbezeichnungen", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "TitelId", "dbo.Titels");
            DropForeignKey("dbo.Users", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Users", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Users", "GeschlechtID", "dbo.Geschlechter");
            DropForeignKey("dbo.Users", "DienstbezeichnungId", "dbo.Dienstbezeichnungen");
            DropForeignKey("dbo.UserDienstbezeichnungen", "DienstbezeichnungId", "dbo.Dienstbezeichnungen");
            DropForeignKey("dbo.Rollen", "AdminStatus_AdminStatusID", "dbo.AdminStatus");
            DropForeignKey("dbo.Rollen", "User_UserId", "dbo.Users");
            DropIndex("dbo.Rollen", new[] { "AdminStatus_AdminStatusID" });
            DropIndex("dbo.Rollen", new[] { "User_UserId" });
            DropIndex("dbo.UserDienstbezeichnungen", new[] { "DienstbezeichnungId" });
            DropIndex("dbo.UserDienstbezeichnungen", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "DienstbezeichnungId" });
            DropIndex("dbo.Users", new[] { "StatusId" });
            DropIndex("dbo.Users", new[] { "PositionId" });
            DropIndex("dbo.Users", new[] { "TitelId" });
            DropIndex("dbo.Users", new[] { "GeschlechtID" });
            DropTable("dbo.Rollen");
            DropTable("dbo.ProgrammSettings");
            DropTable("dbo.Titels");
            DropTable("dbo.Status");
            DropTable("dbo.Positions");
            DropTable("dbo.Geschlechter");
            DropTable("dbo.UserDienstbezeichnungen");
            DropTable("dbo.Dienstbezeichnungen");
            DropTable("dbo.Users");
            DropTable("dbo.AdminStatus");
        }
    }
}
