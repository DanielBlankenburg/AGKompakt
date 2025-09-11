using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    internal class Postions
    {
        public string Name { get; set; }
        public string ID;
        public Postions(string name, string id)
        { Name = name; ID = id; }
    }
}
