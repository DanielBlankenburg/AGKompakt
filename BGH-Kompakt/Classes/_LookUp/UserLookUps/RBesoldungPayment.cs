using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.UserLookUps
{
    public  class RBesoldungPayment
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public decimal PaymentValue { get; set; }
        public RBesoldung RBesoldung {  get; set; }
    }
}
