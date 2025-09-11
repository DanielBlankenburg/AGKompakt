using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Task = System.Threading.Tasks.Task;
using Word = Microsoft.Office.Interop.Word;

namespace BGH_Kompakt.Converter
{
    public static class DocxToPDFConverter
    {

        public static async Task ConvertFileMultiFolderAsync(string path)
        {
            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            string VerfahrenName = string.Empty;
            List<Task> allTasks = new List<Task>();

            List<string> dirVerfahren = new List<string>(Directory.EnumerateDirectories(path));
            foreach (var dirV in dirVerfahren)
            {
                VerfahrenName = dirV.Substring(dirV.LastIndexOf("\\") + 1);
                if (path.Substring(path.Length - 1) != "\\")
                    path = path + "\\";
                string pathVerfahren = path + VerfahrenName;

                //await ConvertFile (path);
                List<string> dirUnterlagen = new List<string>(Directory.EnumerateDirectories(pathVerfahren));

                foreach (string item in dirUnterlagen)
                {
                    FileAttributes attr = File.GetAttributes(item);
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        foreach (FileInfo file in new DirectoryInfo(item).GetFiles())
                        {
                            if (file.Extension == ".docx" || file.Extension == ".docm")
                            {
                                if (file.Name.Substring(0, 7) != "Verweis")
                                {
                                    allTasks.Add(Task.Run(() => ConvertDOCtoPDFasync(item, file.Name, file.Extension)));
                                }
                            }
                        }
                    }
                    else
                    {
                        FileInfo file = new FileInfo(item);
                        if (file.Extension == ".docx" || file.Extension == ".docm")
                        {
                            if (file.Name.Substring(0, 7) != "Verweis")
                            {
                                allTasks.Add(Task.Run(() => ConvertDOCtoPDFasync(pathVerfahren, file.Name, file.Extension)));
                            }
                        }
                    }
                }
            }
            await Task.WhenAll(allTasks);
        }

        public static async Task ConvertFileSingleFolderAsync(string path)
        {
            List<Task> allTasks = new List<Task>();
            foreach (FileInfo file in new DirectoryInfo(path).GetFiles())
            {
                if (file.Extension == ".docx" || file.Extension == ".docm")
                {
                    if (file.Name.Substring(0, 7) != "Verweis")
                    {
                        allTasks.Add(Task.Run(() => ConvertDOCtoPDFasync(path, file.Name, file.Extension)));
                    }
                }
            }
            await Task.WhenAll(allTasks);
        }

        public static void ConvertDOCtoPDFasync(string path, string fileName, string extension)
        {
            string rohdateiname = fileName.Substring(0, fileName.Length - 5);
            //MessageBox.Show(rohdateiname);
            try
            {
                string filename = path + "\\" + rohdateiname + ".pdf";
                if (!File.Exists(filename))
                {
                    if (File.Exists(path + "\\" + rohdateiname + extension))
                    {
                        object misValue = System.Reflection.Missing.Value;
                        String PATH_APP_PDF = filename;

                        var WORD = new Word.Application();

                        Word.Document doc = WORD.Documents.Open(path + "\\" + rohdateiname + extension);
                        //doc.Activate();
                        doc.ExportAsFixedFormat(filename, WdExportFormat.wdExportFormatPDF, false, WdExportOptimizeFor.wdExportOptimizeForPrint, WdExportRange.wdExportAllDocument);
                        //doc.SaveAs2(@PATH_APP_PDF, Word.WdSaveFormat.wdFormatPDF, misValue, misValue, misValue,
                        //misValue, misValue, misValue, misValue, misValue, misValue, misValue);

                        doc.Close();
                        WORD.Quit();

                        ReleaseObject(doc);
                        ReleaseObject(WORD);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Die Konvertierung der Datei: " + fileName + " konnte nicht erfolgen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                //TODO
            }
            finally
            {
                GC.Collect();
            }
        }



    }
}
