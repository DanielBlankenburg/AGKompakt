using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Test
{
    public class ItemPresenter : ViewModelBase
    {
        private readonly string _value;

        public ItemPresenter(string value)
        {
            _value = value;
            _ausgabeText = value;
        }

        private string _ausgabeText;
        public string Ausgabetext
        {
            get { return _ausgabeText; }
            set { SetProperty<string>(ref _ausgabeText, value); }
        }

        public override string ToString()
        {
            return _value;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty<bool>(ref _isSelected, value); }
            
        }
    }

    
}
