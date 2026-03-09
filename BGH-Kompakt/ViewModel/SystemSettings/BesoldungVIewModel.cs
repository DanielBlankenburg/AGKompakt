using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.SystemSettings
{
    public partial class BesoldungVIewModel : ViewModelBase
    {
        public ICommand NewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }


        public ObservableCollection<RBesoldung> Besoldungsliste { get; set; } = new ObservableCollection<RBesoldung>();
        public ObservableCollection<RBesoldungPayment> BesoldungPaymentsList { get; set; } = new ObservableCollection<RBesoldungPayment>();
        private readonly UserDBContext UserDBContext = new UserDBContext();

        private bool _NewPayment;
        public bool NewPayment
        {
            get { return _NewPayment; }
            set { SetProperty(ref _NewPayment, value);}
        }



        public BesoldungVIewModel()
        {
            var query = UserDBContext.RBesoldungen.ToList();
            foreach (var item in query) Besoldungsliste.Add(item);
            SaveCommand = new RelayCommand(SaveExecute, SaveCanExecute);
            NewCommand = new RelayCommand(NewExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, DeleteCanExecute);
        }

        RBesoldung _selectedBesoldung;
        public RBesoldung SelectedBesoldung
        {
            get { return _selectedBesoldung; }
            set
            {
                SetProperty(ref _selectedBesoldung, value);
                if (SelectedBesoldung != null)
                {
                    BesoldungPaymentsList.Clear();
                    var query = UserDBContext.RBesoldungPayments.Where(x => x.RBesoldung.id == SelectedBesoldung.id).OrderBy(x => x.Start).ToList();
                    foreach (var item in query) BesoldungPaymentsList.Add(item);
                }
            }
        }

        RBesoldungPayment _selectBesoldungPayment;
        public RBesoldungPayment SelectBesoldungPayment
        {
            get { return _selectBesoldungPayment; }
            set
            {
                SetProperty(ref _selectBesoldungPayment, value);
                if (value != null) NewPayment = false;
            }
        }
        private void NewExecute(object obj)
        {
            if (SelectedBesoldung != null)
            {
                SelectBesoldungPayment = new RBesoldungPayment();
                NewPayment = true;
            }
            else
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie zunächst eine Besoldungsgruppe aus.", false);
            }
        }
        private void SaveExecute(object obj)
        {
            try
            {
                if (SelectedBesoldung == null)
                {
                    ViewManager.ShowMainInfoFlyout("Bitte wählen Sie zunächst eine Besoldungsgruppe aus.", false);
                    return;
                }
                if (SelectBesoldungPayment.Start < new DateTime(2025, 1, 1))
                {
                    ViewManager.ShowMainInfoFlyout("Das Gültigkeitsdatum muss nach dem 1. Januar 2025 liegen. Bitte ändern Sie dieses entsprechend ab.", false);
                    return;
                }
                if (NewPayment)
                {
                    SelectBesoldungPayment.RBesoldung = SelectedBesoldung;
                    BesoldungPaymentsList.Add(SelectBesoldungPayment);
                    NewPayment = false;
                }
                else
                {
                    RBesoldungPayment editPayment = UserDBContext.RBesoldungPayments.FirstOrDefault(x => x.Id == SelectBesoldungPayment.Id);
                    if (editPayment == null)
                    {
                        ErrorMessage.CreateExceptionWithFlyOutMessage("SaveExecute", new Exception("Die ausgewählte Besoldung konnte nicht gespeichert werden, da sie in der Datenbank nicht gefunden wurde."));
                        return;
                    }
                }
                UserDBContext.RBesoldungPayments.AddOrUpdate(SelectBesoldungPayment);
                UserDBContext.SaveChanges();
            }
            catch (Exception ex) { ErrorMessage.CreateExceptionWithFlyOutMessage("SaveExecute", ex); }

        }
        private bool SaveCanExecute(object obj)
        {
            return SelectBesoldungPayment != null || NewPayment;
        }
        private void DeleteExecute(object obj)
        {
            if (BesoldungPaymentsList.Count < 2)
            {
                ViewManager.ShowMainInfoFlyout("Es muss mindestens ein Eintrag vorhanden sein. Bitte fügen Sie zunächst einen neuen Eintrag hinzu. Danach können Sie diesen Eintrag löschen.", false);
                return;
            }
            if (ViewManager.ShowQuestionWindow("Soll der Eintrag gelöscht werden?", "ja"))
            {
                try
                {
                    RBesoldungPayment deletePayment = UserDBContext.RBesoldungPayments.FirstOrDefault(x => x.Id == SelectBesoldungPayment.Id);
                    if (deletePayment == null)
                    {
                        ErrorMessage.CreateExceptionWithFlyOutMessage("SaveExecute", new Exception("Die ausgewählte Besoldung konnte nicht gelöscht werden, da sie in der Datenbank nicht gefunden wurde."));
                        return;
                    }
                    UserDBContext.RBesoldungPayments.Remove(SelectBesoldungPayment);
                    UserDBContext.SaveChanges();
                    BesoldungPaymentsList.Remove(deletePayment);
                    SelectBesoldungPayment = new RBesoldungPayment();
                    NewPayment = true;

                }
                catch (Exception ex) { ErrorMessage.CreateExceptionWithFlyOutMessage("DeleteExecute", ex); }
            }
        }

        private bool DeleteCanExecute(object obj)
        {
            return !NewPayment && SelectBesoldungPayment != null;
        }


    }
}
