using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Sitzungsunterlagen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Sitzungsunterlagen
{
    public class BSCWServerViewModel : ViewModelBase
    {
        public ICommand BackCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ExportBSCWServerCommand { get; set; }

        public List<CompareBSCW> Filelist { get; set; } = new List<CompareBSCW>();
        public ObservableCollection<CompareBSCW> FileListShow { get; set; } = new ObservableCollection<CompareBSCW>();
        private bool ShowOnlyMissingFiles { get; set; } = false;
        private string _FilterButtonTitle = "Nur fehlende Dateien anzeigen";
        public string FilterButtonTitle
        {
            get { return _FilterButtonTitle; }
            set
            {
                SetProperty(ref _FilterButtonTitle, value);
            }
        }

        public BSCWServerViewModel()
        {
            BackCommand = new RelayCommand(BackExecute);
            FilterCommand = new RelayCommand(FilterExecute);
            ExportBSCWServerCommand = new RelayCommand(ExportBSCWServerExecute, ExportBSCWServerCanExecute);

            Filelist = ViewManager.Filelist;
            foreach (var x in Filelist) FileListShow.Add(x); 
        }

        private bool ExportBSCWServerCanExecute(object obj)
        {
            List<CompareBSCW> ErrorList = new List<CompareBSCW>();
            foreach (var x in Filelist)
            {
                if (!x.FileExists) ErrorList.Add(x);
            }
            return ErrorList.Count > 0;
            //return true;
        }
        
        private async void ExportBSCWServerExecute(object obj)
        {
            string actionName = "Übertragung BSCW-Server";
            Task<DBResponse> task = ExportBSCWServerFilesAsync();
            ViewManager.ShowMainInfoFlyout("Die Dateien werden übertragen. Sie können dabei mit dem Programm weiterarbeiten.", false);
            ViewManager.ActionlistAdd(actionName);
            ViewManager.ShowPageOnMainView<SitzungsunterlagenView>();

            await task;
            string message = task.Result.Success ? "Die fehlenden Unterlagen wurden auf den BSCW-Server übertragen." : task.Result.Message;
            ViewManager.ShowMainInfoFlyout(message, false);
            ViewManager.ActionlistRemove(actionName);

        }

        private Task<DBResponse> ExportBSCWServerFilesAsync()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() => 
            {
                string ErrorFiles = string.Empty;
                foreach (var file in Filelist)
                {
                    if (!file.FileExists)
                    {
                        try
                        {
                            FileInfo fileInfo = new FileInfo(file.FilePath);
                            string fileName = fileInfo.Name;
                            string path = fileInfo.FullName;

                            string folderOfInterest = "Sitzungsunterlagen";

                            var directories = path.Split('\\')
                                .SkipWhile(x => x != folderOfInterest);

                            string exportFile = string.Join("\\", directories);
                            exportFile = $"{UserManager.SenatSettings.BSCW_Server_Drive}:{exportFile.Substring(folderOfInterest.Length)}";
                            var dirExport = exportFile.Split('\\');
                            string Testpath = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\";
                            for (int i = 1; i < dirExport.Length - 1; i++)
                            {
                                Testpath += $"{dirExport[i]}\\";
                                if (!Directory.Exists(Testpath)) Directory.CreateDirectory(Testpath);
                            }
                            //MessageBox.Show($"{exportFile}");
                            File.Copy(fileInfo.FullName, exportFile, true);
                        }
                        catch 
                        {
                            ErrorFiles += $"{file}{(ErrorFiles == string.Empty ? "; " : " ")}";
                        }
                    }
                }

                DBResponse resp = new DBResponse 
                { 
                    Success = ErrorFiles == string.Empty, 
                    Message = ErrorFiles == string.Empty ? "" : $"Es konnten folgende Dateien nicht übertragen werden:\n\n{ErrorFiles}" 
                };
                return resp;
            });
            return task;
        }

        private void FilterExecute(object obj)
        {
            FileListShow.Clear();
            if (!ShowOnlyMissingFiles)
            {
                ShowOnlyMissingFiles = true;
                foreach (var x in Filelist) if (!x.FileExists) FileListShow.Add(x);
                FilterButtonTitle = "Alle Dateien anzeigen";
            }
            else
            {
                ShowOnlyMissingFiles = false;
                foreach (var x in Filelist) FileListShow.Add(x);
                FilterButtonTitle = "Nur fehlende Dateien anzeigen";
            }

        }

        private void BackExecute(object obj)
        {
            ViewManager.ShowPageOnMainView<SitzungsunterlagenView>();
        }
    }
}
