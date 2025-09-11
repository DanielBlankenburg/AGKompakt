using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.MP
{
    public class MPEMailRecipient
    {
        public int MPEMailRecipientID { get; set; }
        public string MPEMailRecipientAdress { get; set; }
        public int MPEMailRecipientTyp { get; set; } //1 = Word; 2 = pdf
        public MPEMailRecipient()
        {

        }
        public MPEMailRecipient(string recipient, int typ)
        {
            MPEMailRecipientAdress = recipient;
            MPEMailRecipientTyp = typ;
        }
    }
}
