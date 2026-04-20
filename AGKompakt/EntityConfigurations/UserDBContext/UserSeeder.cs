namespace BGH_Kompakt.EntityConfigurations.UserDBContext
{
    public class UserSeeder
    {

        //public UserSeeder(BGH_Kompakt.Services.DBContexts.UserDBContext context)
        //{
        //    Context = context;
        //}

        //public void Dienstbezeichnungen()
        //{
        //    Context.Dienstbezeichnungen.AddOrUpdate( a => a.DienstbezeichnungText, 
        //        new Dienstbezeichnung { DienstbezeichnungText = "RiBGH"}, 
        //        new Dienstbezeichnung { DienstbezeichnungText = "RinBGH" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "VRiBGH" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "VRinBGH" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "RiOLG" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "RinOLG" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "RiLG" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "RinLG" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "RiAG" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "RinAG" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "DirAG" },
        //        new Dienstbezeichnung { DienstbezeichnungText = "DirinAG" }
        //        );
        //}

        //public void Geschlechter()
        //{
        //    Context.Geschlechter.AddOrUpdate(a => a.GeschlechtText,
        //        new Geschlecht { GeschlechtText = "männlich"},
        //        new Geschlecht { GeschlechtText = "weiblich"},
        //        new Geschlecht { GeschlechtText = "divers"}
        //        );
        //}

        //public void Posítions()
        //{
        //    Context.Positions.AddOrUpdate(a => a.PositionText,
        //        new Position { PositionText= "Richter am Bundesgerichtshof" },
        //        new Position { PositionText = "Wissenschaftlicher Mitarbeiter" },
        //        new Position { PositionText = "Geschäftsstellenmitarbeiter" }
        //        );
        //}
        //public void Status()
        //{
        //    Context.Status.AddOrUpdate(a => a.StatusText,
        //        new Status { StatusText= "aktiv" },
        //        new Status { StatusText = "inaktiv" }
        //        );
        //}       
        //public void Titel()
        //{
        //    Context.Titel.AddOrUpdate(a => a.TitelText,
        //        new Titel { TitelText = "kein Titel" },
        //        new Titel { TitelText = "Dr." },
        //        new Titel { TitelText = "Prof. Dr." }
        //        );
        //}

        //public void Senate()
        //{
        //    //Muss mit MPSenate synchronisiert werden
        //    Context.Senate.AddOrUpdate(a => a.SenatName,
        //        new Senat { SenatName = "unbekannter Senat", SenatArt = 0 },
        //        new Senat { SenatName = "I. Zivilsenat", SenatArt = 1, SenatShort = "I" },
        //        new Senat { SenatName = "II. Zivilsenat", SenatArt = 1, SenatShort = "II" },
        //        new Senat { SenatName = "III. Zivilsenat", SenatArt = 1, SenatShort = "III" },
        //        new Senat { SenatName = "IV. Zivilsenat", SenatArt = 1, SenatShort = "IV" },
        //        new Senat { SenatName = "V. Zivilsenat", SenatArt = 1, SenatShort = "V" },
        //        new Senat { SenatName = "VI. Zivilsenat", SenatArt = 1, SenatShort = "VI" },
        //        new Senat { SenatName = "VIa. Zivilsenat", SenatArt = 1, SenatShort = "VIa" },
        //        new Senat { SenatName = "VII. Zivilsenat", SenatArt = 1, SenatShort = "VII" },
        //        new Senat { SenatName = "VIII. Zivilsenat", SenatArt = 1, SenatShort = "VIII" },
        //        new Senat { SenatName = "IX. Zivilsenat", SenatArt = 1, SenatShort = "IX", Path = "k:" },
        //        new Senat { SenatName = "X. Zivilsenat", SenatArt = 1, SenatShort = "X", Path = "l:" },
        //        new Senat { SenatName = "XI. Zivilsenat", SenatArt = 1, SenatShort = "XI" },
        //        new Senat { SenatName = "XII. Zivilsenat", SenatArt = 1, SenatShort = "XII" },
        //        new Senat { SenatName = "XIII. Zivilsenat", SenatArt = 1, SenatShort = "XIII" },
        //        new Senat { SenatName = "1. Strafsenat", SenatArt = 2, SenatShort = "1" },
        //        new Senat { SenatName = "2. Strafsenat", SenatArt = 2, SenatShort = "2" },
        //        new Senat { SenatName = "3. Strafsenat", SenatArt = 2, SenatShort = "3" },
        //        new Senat { SenatName = "4. Strafsenat", SenatArt = 2, SenatShort = "4" },
        //        new Senat { SenatName = "5. Strafsenat", SenatArt = 2, SenatShort = "5" },
        //        new Senat { SenatName = "6. Strafsenat", SenatArt = 2, SenatShort = "6" },
        //        new Senat { SenatName = "Anwaltssenat", SenatArt = 3 },
        //        new Senat { SenatName = "Notarsenat", SenatArt = 3 },
        //        new Senat { SenatName = "Landwirtschaftssenat", SenatArt = 3 },
        //        new Senat { SenatName = "Patentanwaltsenat", SenatArt = 3 },
        //        new Senat { SenatName = "Steuerberatersenat", SenatArt = 3 },
        //        new Senat { SenatName = "Gemeinsamer Senat der obersten Gerichtshöfe", SenatArt = 3 },
        //        new Senat { SenatName = "Großer Zivilsenat", SenatArt = 3 },
        //        new Senat { SenatName = "Großer Strafsenat", SenatArt = 3 },
        //        new Senat { SenatName = "Dienstgericht", SenatArt = 3 }
        //    );
        //}

        //public void AdminStatus()
        //{
        //    Context.AdminStatus.AddOrUpdate(i => i.AdminStatusText,
        //        new AdminStatus { AdminStatusText = UserEnums.EnumAdminStatus.Programm.ToString()},
        //        new AdminStatus { AdminStatusText = UserEnums.EnumAdminStatus.Nebentätigkeiten.ToString() },
        //        new AdminStatus { AdminStatusText = UserEnums.EnumAdminStatus.MontagspostAdmin.ToString() },
        //        new AdminStatus { AdminStatusText = UserEnums.EnumAdminStatus.Präsidentin.ToString() },
        //        new AdminStatus { AdminStatusText = UserEnums.EnumAdminStatus.Präsidialrichter.ToString() },
        //        new AdminStatus { AdminStatusText = UserEnums.EnumAdminStatus.Vorzimmer.ToString() }
        //        );
        //}
        
        //public void Users()
        //{
        //    Context.Users.AddOrUpdate(u => u.VorName,
        //        new User { VorName = "Max", NachName = "Präsidialrichter", EMail = "Mustermann@gmx.de", ComputerName = "Test1", GeschlechtID = 1, PositionId = 1, StatusId = 1, DienstbezeichnungId = 1 },
        //        new User { VorName = "Maria", NachName = "Präsidentin", EMail = "Musterfrau@gmx.de", ComputerName = "Test2", GeschlechtID = 2, PositionId = 1, StatusId = 1, DienstbezeichnungId = 1 },
        //        new User { VorName = "Test", NachName = "Vorzimmer", EMail = "Zimmermann@gmx.de", ComputerName = "Test3", GeschlechtID = 1, PositionId = 1, StatusId = 1, DienstbezeichnungId = 1 }
        //    );
        //}

    }
}
