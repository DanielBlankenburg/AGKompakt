using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Vintages : ViewModelBase
    {
        private string _Jahr; 
        public string Jahr
        {
            get { return _Jahr; }
            set { SetProperty(ref _Jahr, value); }
        }

        public Vintages(string jahr) => Jahr = jahr;
    }
}
