using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.ViewModel;
using System.Collections.Generic;

namespace BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps
{
    public class ActivityRequestScienceAuthorName : ViewModelBase
    {
        public int ActivityRequestScienceAuthorNameId { set; get; }
        public string ActivityRequestScienceAuthorText { set; get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty<bool>(ref _isSelected, value); }

        }

    }
}

