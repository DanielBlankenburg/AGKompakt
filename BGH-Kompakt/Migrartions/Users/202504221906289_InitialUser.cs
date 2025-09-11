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
                "dbo.UserFilterMP",
                c => new
                    {
                        UserFilterMPID = c.Int(nullable: false),
                        ZivilGesamt = c.Boolean(nullable: false),
                        ZivilSenat1 = c.Boolean(nullable: false),
                        ZivilSenat2 = c.Boolean(nullable: false),
                        ZivilSenat3 = c.Boolean(nullable: false),
                        ZivilSenat4 = c.Boolean(nullable: false),
                        ZivilSenat5 = c.Boolean(nullable: false),
                        ZivilSenat6 = c.Boolean(nullable: false),
                        ZivilSenat6a = c.Boolean(nullable: false),
                        ZivilSenat7 = c.Boolean(nullable: false),
                        ZivilSenat8 = c.Boolean(nullable: false),
                        ZivilSenat9 = c.Boolean(nullable: false),
                        ZivilSenat10 = c.Boolean(nullable: false),
                        ZivilSenat11 = c.Boolean(nullable: false),
                        ZivilSenat12 = c.Boolean(nullable: false),
                        ZivilSenat13 = c.Boolean(nullable: false),
                        StrafGesamt = c.Boolean(nullable: false),
                        StrafSenat1 = c.Boolean(nullable: false),
                        StrafSenat2 = c.Boolean(nullable: false),
                        StrafSenat3 = c.Boolean(nullable: false),
                        StrafSenat4 = c.Boolean(nullable: false),
                        StrafSenat5 = c.Boolean(nullable: false),
                        StrafSenat6 = c.Boolean(nullable: false),
                        SonderGesamt = c.Boolean(nullable: false),
                        GmSOG = c.Boolean(nullable: false),
                        GZS = c.Boolean(nullable: false),
                        GStS = c.Boolean(nullable: false),
                        Anwaltssenat = c.Boolean(nullable: false),
                        Patentanwaltssenat = c.Boolean(nullable: false),
                        Notarsenat = c.Boolean(nullable: false),
                        Steuerberater = c.Boolean(nullable: false),
                        Dienstgericht = c.Boolean(nullable: false),
                        Kartellsenat = c.Boolean(nullable: false),
                        Landwirtschaftssenat = c.Boolean(nullable: false),
                        Urteile = c.Boolean(nullable: false),
                        Beschluesse = c.Boolean(nullable: false),
                        Leitsatzentscheidung = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserFilterMPID)
                .ForeignKey("dbo.Users", t => t.UserFilterMPID)
                .Index(t => t.UserFilterMPID);
            
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
                "dbo.Senate",
                c => new
                    {
                        SenatID = c.Int(nullable: false, identity: true),
                        SenatName = c.String(),
                        SenatShort = c.String(),
                        SenatArt = c.Int(nullable: false),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.SenatID);
            
            CreateTable(
                "dbo.SenatSettings",
                c => new
                    {
                        SenatSettingID = c.Int(nullable: false),
                        SenatID = c.Int(),
                        ShowSitzungsplaene = c.Boolean(nullable: false),
                        ShowVerteilung = c.Boolean(nullable: false),
                        ShowVotenmappe = c.Boolean(nullable: false),
                        ShowSpruchgruppen = c.Boolean(nullable: false),
                        ShowFormerDays = c.Boolean(nullable: false),
                        BSCW_Server_Drive = c.Int(),
                        AZPrefix = c.Boolean(nullable: false),
                        AZPrefixDate = c.Boolean(nullable: false),
                        AZPrefixChar = c.String(),
                        ImportAGUrteil = c.String(),
                        ImportAGBeschluss = c.String(),
                        ImportLGUrteil = c.String(),
                        ImportLGBeschluss = c.String(),
                        ImportLGHB = c.String(),
                        ImportLGZB = c.String(),
                        ImportOLGUrteil = c.String(),
                        ImportOLGBeschluss = c.String(),
                        ImportOLGHB = c.String(),
                        ImportOLGZB = c.String(),
                        ImportEUGHVorlage = c.String(),
                        ImportEUGHURteil = c.String(),
                        ImportEntwurf = c.String(),
                        ImportVotum = c.String(),
                        ImportVorVotum = c.String(),
                        ImportAnlage = c.String(),
                        ImportLeitsatz = c.String(),
                        ImportRMB = c.String(),
                        ImportRME = c.String(),
                        ImportSonstiges = c.String(),
                    })
                .PrimaryKey(t => t.SenatSettingID)
                .ForeignKey("dbo.Senate", t => t.SenatSettingID)
                .Index(t => t.SenatSettingID);
            
            CreateTable(
                "dbo.SenatAktenzeichen",
                c => new
                    {
                        SenatAktenzeichenID = c.Int(nullable: false, identity: true),
                        SenatAktenzeichenName = c.String(),
                        SenatAktenzeichenOrderNumber = c.Int(nullable: false),
                        SenatSetting_SenatSettingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SenatAktenzeichenID)
                .ForeignKey("dbo.SenatSettings", t => t.SenatSetting_SenatSettingID, cascadeDelete: true)
                .Index(t => t.SenatSetting_SenatSettingID);
            
            CreateTable(
                "dbo.SenatSpruchgruppen",
                c => new
                    {
                        SenatSpruchgruppeID = c.Int(nullable: false, identity: true),
                        SenatSpruchgruppeName = c.String(),
                        SenatSpruchgruppeOrderNumber = c.Int(nullable: false),
                        SenatSetting_SenatSettingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SenatSpruchgruppeID)
                .ForeignKey("dbo.SenatSettings", t => t.SenatSetting_SenatSettingID, cascadeDelete: true)
                .Index(t => t.SenatSetting_SenatSettingID);
            
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
                "dbo.Votenmappe",
                c => new
                    {
                        VerfahrenVotenmappeID = c.Int(nullable: false, identity: true),
                        Verfahren_FullPath = c.String(),
                        Verfahren_Anzeigedaten = c.String(),
                    })
                .PrimaryKey(t => t.VerfahrenVotenmappeID);
            
            CreateTable(
                "dbo.UserAdminStatus",
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
            
            CreateTable(
                "dbo.SenatUsers",
                c => new
                    {
                        Senat_SenatID = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Senat_SenatID, t.User_UserId })
                .ForeignKey("dbo.Senate", t => t.Senat_SenatID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Senat_SenatID)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "TitelId", "dbo.Titels");
            DropForeignKey("dbo.Users", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SenatUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.SenatUsers", "Senat_SenatID", "dbo.Senate");
            DropForeignKey("dbo.SenatSettings", "SenatSettingID", "dbo.Senate");
            DropForeignKey("dbo.SenatSpruchgruppen", "SenatSetting_SenatSettingID", "dbo.SenatSettings");
            DropForeignKey("dbo.SenatAktenzeichen", "SenatSetting_SenatSettingID", "dbo.SenatSettings");
            DropForeignKey("dbo.Users", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Users", "GeschlechtID", "dbo.Geschlechter");
            DropForeignKey("dbo.UserFilterMP", "UserFilterMPID", "dbo.Users");
            DropForeignKey("dbo.Users", "DienstbezeichnungId", "dbo.Dienstbezeichnungen");
            DropForeignKey("dbo.UserAdminStatus", "AdminStatus_AdminStatusID", "dbo.AdminStatus");
            DropForeignKey("dbo.UserAdminStatus", "User_UserId", "dbo.Users");
            DropIndex("dbo.SenatUsers", new[] { "User_UserId" });
            DropIndex("dbo.SenatUsers", new[] { "Senat_SenatID" });
            DropIndex("dbo.UserAdminStatus", new[] { "AdminStatus_AdminStatusID" });
            DropIndex("dbo.UserAdminStatus", new[] { "User_UserId" });
            DropIndex("dbo.SenatSpruchgruppen", new[] { "SenatSetting_SenatSettingID" });
            DropIndex("dbo.SenatAktenzeichen", new[] { "SenatSetting_SenatSettingID" });
            DropIndex("dbo.SenatSettings", new[] { "SenatSettingID" });
            DropIndex("dbo.UserFilterMP", new[] { "UserFilterMPID" });
            DropIndex("dbo.Users", new[] { "DienstbezeichnungId" });
            DropIndex("dbo.Users", new[] { "StatusId" });
            DropIndex("dbo.Users", new[] { "PositionId" });
            DropIndex("dbo.Users", new[] { "TitelId" });
            DropIndex("dbo.Users", new[] { "GeschlechtID" });
            DropTable("dbo.SenatUsers");
            DropTable("dbo.UserAdminStatus");
            DropTable("dbo.Votenmappe");
            DropTable("dbo.Titels");
            DropTable("dbo.Status");
            DropTable("dbo.SenatSpruchgruppen");
            DropTable("dbo.SenatAktenzeichen");
            DropTable("dbo.SenatSettings");
            DropTable("dbo.Senate");
            DropTable("dbo.Positions");
            DropTable("dbo.Geschlechter");
            DropTable("dbo.UserFilterMP");
            DropTable("dbo.Dienstbezeichnungen");
            DropTable("dbo.Users");
            DropTable("dbo.AdminStatus");
        }
    }
}
