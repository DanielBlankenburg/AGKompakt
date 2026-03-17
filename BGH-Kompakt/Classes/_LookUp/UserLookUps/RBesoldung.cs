using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.UserLookUps
{
    public class RBesoldung
    {
        public int id { get; set; }
        public string Name { get; set; }
        public IList<Dienstbezeichnung> Dienstbezeichnungen { get; set; }
        public IList<RBesoldungPayment> RBesoldungPayments { get; set; }
    }
}
