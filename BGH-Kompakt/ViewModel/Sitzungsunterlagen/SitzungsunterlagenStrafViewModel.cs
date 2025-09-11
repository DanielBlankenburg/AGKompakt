using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.Test;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.Interfaces;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Sitzungsunterlagen
{
    public class SitzungsunterlagenStrafViewModel : ViewModelBase, IFilesDropped
    {
        private readonly UserDBContext userDBContext = new UserDBContext();
        private DBResponse resp = new DBResponse();

        public ICommand RenameCommand { get; set; }
        public ICommand DeleteAttechmentCommand { get; set; }
        public ICommand OpenAttechmentCommand { get; set; }

        #region Test-Grouping
        public ObservableCollection<TestItem> Items { get; set; }
        public CollectionViewSource CollectionView { get; set; }

        #endregion


        public SitzungsunterlagenStrafViewModel()
        {
            RenameCommand = new RelayCommand(RenameExecute);
            DeleteAttechmentCommand = new RelayCommand(DeleteAttechmentExecute);
            OpenAttechmentCommand = new RelayCommand(OpenAttechmentExecute);

            string year = DateTime.Now.Year.ToString();
            TextJahr = year.Substring(2);
            FillComoboboxes();

            //Testgrouping
            List<TestItem> clients = new List<TestItem>();
            ActivityRequestDBContext activityRequestDBContext = new ActivityRequestDBContext();
            var Client_Query = activityRequestDBContext.ActivityClients.Include(t => t.ActivityClientTyp).OrderBy(ac => ac.ActivityClientTypID).ThenBy(ac => ac.ACName);
            foreach (var item in Client_Query) clients.Add(new TestItem() { Name = item.ACName, Category= item.ActivityClientTyp.ActivityClientTypText});
            Items = new ObservableCollection<TestItem>(clients);
            var view = new CollectionViewSource();
            view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            view.Source = Items;
            CollectionView = view;
        }


        private void FillComoboboxes()
        {
            //Senate füllen
            resp = Cbo_Senat_Fill(SenatList);
            if (!resp.Success)
            {
                ViewManager.ShowMainInfoFlyout(resp.Message, false); 
                return;
            }
            //Aktenzeichen füllen
            resp = Cbo_Aktenzeichen_Fill(AZList);
            if (!resp.Success)
            {
                ViewManager.ShowMainInfoFlyout(resp.Message, false); 
                return;
            }

            //Beisitzer füllen
            resp = Cbo_Beisitzer_Fill(ListBeisitzer);
            if (!resp.Success)
            {
                ViewManager.ShowMainInfoFlyout(resp.Message, false); 
                return;
            }


        }

        private ObservableCollection<MPImportFile> _ImportFileList = new ObservableCollection<MPImportFile>();
        public ObservableCollection<MPImportFile> ImportFileList { get { return _ImportFileList; } }

        public ObservableCollection<User> ListBeisitzer { get; set; } = new ObservableCollection<User>();

        private User _SelectedBeisitzer;
        public User SelectedBeisitzer
        {
            get { return _SelectedBeisitzer; }
            set
            {
                SetProperty(ref _SelectedBeisitzer, value);
            }
        }

        public ObservableCollection<Senat> SenatList { get; set; } = new ObservableCollection<Senat>();
        private Senat _SelectedSenat;
        public Senat SelectedSenat
        {
            get { return _SelectedSenat; }
            set
            {
                SetProperty(ref _SelectedSenat, value);
                //AZIsEnable = true;
                //AZList.Clear();
                //if (SelectedSenat != null)
                //{
                //    var Query = userDBContext.SenatAktenzeichen.Where(x => x.SenatSetting.SenatID == SelectedSenat.SenatID).ToList();
                //    if (Query.Count > 0) foreach (var item in Query) AZList.Add(item);
                //    var Query2 = userDBContext.SenatSpruchgruppen.Where(x => x.SenatSetting.SenatID == SelectedSenat.SenatID).ToList();
                //    if (Query2.Count > 0) foreach (var item in Query2) SGList.Add(item);
                //}
            }
        }

        public ObservableCollection<SenatAktenzeichen> AZList { get; set; } = new ObservableCollection<SenatAktenzeichen>();
        private SenatAktenzeichen _SelectedAZ;
        public SenatAktenzeichen SelectedAZ
        {
            get { return _SelectedAZ; }
            set {SetProperty(ref _SelectedAZ, value);}
        }



        private bool _ShowAttechmentList = false;
        public bool ShowAttechmentList
        {
            get { return _ShowAttechmentList; }
            set { SetProperty(ref _ShowAttechmentList, value); }
        }

        private MPImportFile _selectedAttechment;
        public MPImportFile SelectedAttechment
        {
            get { return _selectedAttechment; }
            set { _selectedAttechment = value; }
        }

        private string _TextJahr = string.Empty;
        public string TextJahr
        {
            get { return _TextJahr; }
            set
            {
                string pattern = "^[0-9]{2}$";
                Regex rg = new Regex(pattern);
                if (rg.IsMatch(value))
                {
                    SetProperty(ref _TextJahr, value);
                }
                else
                {
                    ViewManager.ShowMainInfoFlyout("Bitte geben Sie nur eine zweistellige Zahl ein.", false);
                }
            }
        }

        private string _TextLaufendeNummer = string.Empty;
        public string TextLaufendeNummer
        {
            get { return _TextLaufendeNummer; }
            set { SetProperty(ref _TextLaufendeNummer, value); }
        }


        public void OnFilesDropped(string[] files)
        {
            foreach (string file in files) ImportFileList.Add(new MPImportFile(file));
            ShowAttechmentList = true;
        }

        #region Executes
        private void RenameExecute(object obj)
        {
            if (ValidateInput())
            {
                string newFileName = $"Aktenauszug_1_StR_{TextLaufendeNummer}_{TextJahr}{SelectedAttechment.FileExtention}";
                MPImportFile selectedFile = SelectedAttechment;
                ObservableCollection<MPImportFile> templist = new ObservableCollection<MPImportFile>();
                foreach (MPImportFile item in ImportFileList) templist.Add(item);
                ImportFileList.Clear();
                foreach(MPImportFile item in templist)
                {
                    if (item.FileName == selectedFile.FileName) item.FileName = newFileName;
                    ImportFileList.Add(item);
                } 
            }
            
        }

        private void OpenAttechmentExecute(object obj)
        {
            try
            {
                Process.Start(new ProcessStartInfo(SelectedAttechment.FileFullPath)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Das Dokument konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }

        }


        private bool ValidateInput()
        {
            string Message = string.Empty;
            if (string.IsNullOrEmpty(TextLaufendeNummer)) Message = "Bitte tragen Sie eine laufende Nummer ein.";
            else if (string.IsNullOrEmpty(TextJahr)) Message = "Bitte tragen Sie ein Jahr ein.";
            if (Message != string.Empty)
            {
                ViewManager.ShowMainInfoFlyout(Message, false);
                return false;
            }
            else return true;

        }

        private void DeleteAttechmentExecute(object obj)
        {
            ImportFileList.Remove(SelectedAttechment);
        }


        #endregion

        #region Fill-Functions
        private DBResponse Cbo_Senat_Fill(ObservableCollection<Senat> iList)
        {
            //Alle Zivilsenat auswählen
            DBResponse resp = new DBResponse();
            try
            {
                iList.Clear();
                var Query = userDBContext.Senate.Include(u => u.Users).Where(x => x.SenatArt == 2).ToList();
                if (Query.Count > 0)
                {
                    foreach (var item in Query)
                    {
                        iList.Add(item);
                        if (item.SenatID == UserManager.SenatSettings.SenatID) SelectedSenat = item;
                    }

                }
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = $"Die Senate konnten nicht eingelesen werden. Es ist folgender Fehler aufgetreten:\n\n {ex.Message}.";
            }
            return resp;
        }

        private DBResponse Cbo_Aktenzeichen_Fill(ObservableCollection<SenatAktenzeichen> iList)
        {
            try
            {
                DBResponse resp = new DBResponse();
                SenatAktenzeichen senatAktenzeichen = new SenatAktenzeichen { SenatAktenzeichenName = "StR" };
                iList.Add(senatAktenzeichen);
                SelectedAZ = senatAktenzeichen;
                iList.Add(new SenatAktenzeichen { SenatAktenzeichenName = "AK" });
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Message = $"Die Aktenzeichen konnten nicht eingelesen werden. Es ist folgender Fehler aufgetreten:\n\n {ex.Message}";
                throw;
            }
            return resp;
        }

        private DBResponse Cbo_Beisitzer_Fill(ObservableCollection<User> iList)
        {
            try
            {
                DBResponse resp = new DBResponse();
                foreach (User user in SelectedSenat.Users) iList.Add(user);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Message = $"Die Aktenzeichen konnten nicht eingelesen werden. Es ist folgender Fehler aufgetreten:\n\n {ex.Message}";
                throw;
            }
            return resp;
        }


        #endregion
    }
}
