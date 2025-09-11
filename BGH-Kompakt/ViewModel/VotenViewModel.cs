using BGH_Kompakt.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.ViewModel
{
    public class VotenViewModel : ViewModelBase
    {
        private Voten _votum;
        private PString _Sitzung;
        private PString _Verfahren;
        private PString _Verfahren_Kurz;
        private PString _Sitzung_Anzeige;
        private PString _Spruchgruppe;
        private string _changed;

        public VotenViewModel(Voten votum)
        {
            if (votum != null)
            {
                _votum = votum;
                initializeFields();

            }
            else
            {
                _votum = new Voten();
                initializeFields();
                _Sitzung.HasChanged = true;
                _Verfahren.HasChanged = true;
                _Verfahren_Kurz.HasChanged = true;
                _Sitzung_Anzeige.HasChanged = true;
                _Spruchgruppe.HasChanged = true;


            }
            _Sitzung.PropertyChanged += votum_PropertyChanged;
            _Verfahren.PropertyChanged += votum_PropertyChanged;
            _Verfahren_Kurz.PropertyChanged += votum_PropertyChanged;
            _Sitzung_Anzeige.PropertyChanged += votum_PropertyChanged;
            _Spruchgruppe.PropertyChanged += votum_PropertyChanged;

        }

        private void votum_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_Sitzung.HasChanged || _Verfahren.HasChanged || _Verfahren_Kurz.HasChanged || _Sitzung_Anzeige.HasChanged || _Spruchgruppe.HasChanged) Changed = "*";
            else Changed = string.Empty;
        }

        private void initializeFields()
        {
            _Sitzung = new PString(_votum.Sitzung);
            _Verfahren = new PString(_votum.Verfahren);
            _Verfahren_Kurz = new PString(_votum.Verfahren_Kurz);
            _Sitzung_Anzeige = new PString(_votum.Sitzung_Anzeige);
            _Spruchgruppe = new PString(_votum.Spruchgruppe);
        }

        public PString Sitzung
        {
            get { return _Sitzung; }
        }

        public PString Verfahren
        {
            get { return _Verfahren; }
        }

        public PString Verfahren_Kurz
        {
            get { return _Verfahren_Kurz; }
        }

        public PString Sitzung_Anzeige
        {
            get { return _Sitzung_Anzeige; }
        }

        public PString Spruchgruppe
        {
            get { return _Spruchgruppe; }
        }

        public string Changed
        {
            get { return _changed; }
            set
            {
                SetProperty<string>(ref _changed, value);
            }
        }

        public void UndoChanges()
        {
            _Sitzung.UndoChanges();
            _Verfahren.UndoChanges();
            _Verfahren_Kurz.UndoChanges();
            _Sitzung_Anzeige.UndoChanges();
            _Spruchgruppe.UndoChanges();
        }

        public void AcceptChanges()
        {
            _Sitzung.AccptChanges();
            _Verfahren.AccptChanges();
            _Verfahren_Kurz.AccptChanges();
            _Sitzung_Anzeige.AccptChanges();
            _Spruchgruppe.AccptChanges();
        }

    }
}
