namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilterLS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFilterMP", "ZivilGesamtLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat1LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat2LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat3LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat4LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat5LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat6LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat6aLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat7LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat8LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat9LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat10LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat11LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat12LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "ZivilSenat13LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "StrafGesamtLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "StrafSenat1LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "StrafSenat2LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "StrafSenat3LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "StrafSenat4LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "StrafSenat5LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "StrafSenat6LS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "SonderGesamtLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "GmSOGLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "VGS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "VGSLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "GZSLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "GStSLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "AnwaltssenatLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "PatentanwaltssenatLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "NotarsenatLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "SteuerberaterLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "DienstgerichtLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "KartellsenatLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "LandwirtschaftssenatLS", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "Wirtschaftspruefersenat", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserFilterMP", "WirtschaftspruefersenatLS", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserFilterMP", "WirtschaftspruefersenatLS");
            DropColumn("dbo.UserFilterMP", "Wirtschaftspruefersenat");
            DropColumn("dbo.UserFilterMP", "LandwirtschaftssenatLS");
            DropColumn("dbo.UserFilterMP", "KartellsenatLS");
            DropColumn("dbo.UserFilterMP", "DienstgerichtLS");
            DropColumn("dbo.UserFilterMP", "SteuerberaterLS");
            DropColumn("dbo.UserFilterMP", "NotarsenatLS");
            DropColumn("dbo.UserFilterMP", "PatentanwaltssenatLS");
            DropColumn("dbo.UserFilterMP", "AnwaltssenatLS");
            DropColumn("dbo.UserFilterMP", "GStSLS");
            DropColumn("dbo.UserFilterMP", "GZSLS");
            DropColumn("dbo.UserFilterMP", "VGSLS");
            DropColumn("dbo.UserFilterMP", "VGS");
            DropColumn("dbo.UserFilterMP", "GmSOGLS");
            DropColumn("dbo.UserFilterMP", "SonderGesamtLS");
            DropColumn("dbo.UserFilterMP", "StrafSenat6LS");
            DropColumn("dbo.UserFilterMP", "StrafSenat5LS");
            DropColumn("dbo.UserFilterMP", "StrafSenat4LS");
            DropColumn("dbo.UserFilterMP", "StrafSenat3LS");
            DropColumn("dbo.UserFilterMP", "StrafSenat2LS");
            DropColumn("dbo.UserFilterMP", "StrafSenat1LS");
            DropColumn("dbo.UserFilterMP", "StrafGesamtLS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat13LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat12LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat11LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat10LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat9LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat8LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat7LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat6aLS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat6LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat5LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat4LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat3LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat2LS");
            DropColumn("dbo.UserFilterMP", "ZivilSenat1LS");
            DropColumn("dbo.UserFilterMP", "ZivilGesamtLS");
        }
    }
}
