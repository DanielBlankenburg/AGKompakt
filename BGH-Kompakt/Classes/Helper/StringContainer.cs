using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Helper
{
    public class StringContainer
    {
        public int IntID { get; set; }
        public string Text { get; set; }

        public StringContainer(int intID, string text)
        {
            IntID = intID;
            Text = text;
        }
    }
}
