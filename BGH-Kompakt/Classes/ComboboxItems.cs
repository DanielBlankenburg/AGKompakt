using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class ComboboxItems
    {
        public int Item_Zahl { get; set; }

        public string Item_Name { get; set; }

        public ComboboxItems(int item_Zahl, string item_Name)
        {
            Item_Zahl = item_Zahl;
            Item_Name = item_Name;
        }
    }
}
