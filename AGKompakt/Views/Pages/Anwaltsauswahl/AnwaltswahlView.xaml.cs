using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;
using BGH_Kompakt.Services.SystemComponents;

namespace BGH_Kompakt.Views
{
    /// <summary>
    /// Interaction logic for AnwaltswahlView.xaml
    /// </summary>
    public partial class AnwaltswahlView : UserControl
    {
        public AnwaltswahlView()
        {
            InitializeComponent();
        }

        private void Btn_pdf_wandlung_Click(object sender, RoutedEventArgs e)
        {
            ConvertDocxToPDF();
        }

        private void Btn_Gesamtpdf_erstellen_Click(object sender, RoutedEventArgs e)
        {
            string path = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathSitzungsunterlagen + "\\" + "Anwaltswahl\\";
            GesamtListeErstellen(path);
        }

        private async void ConvertDocxToPDF()
        {
            //string verfahren = "";
            string path = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathSitzungsunterlagen + "\\" + "Anwaltswahl\\";

            try
            {
                await ConvertFileVerfahren(path);
            }
            catch (Exception)
            {

                MessageBox.Show("Es konnten nicht alle Dateien konvertiert werden.", "Umwandlung", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task ConvertFileVerfahren(string path)
        {

            List<Task> allTasks = new List<Task>();
            foreach (FileInfo file in new DirectoryInfo(path).GetFiles())
            {

                if (file.Name.Contains("docx"))
                {
                    if (file.Name.Contains("docx"))
                    {
                        if (file.Name.Substring(0, 7) != "Verweis")
                        {
                            allTasks.Add(Task.Run(() => convertDOCtoPDFasync(path, file.Name)));
                        }
                    }
                }
            }
            await Task.WhenAll(allTasks);
            ViewManager.ShowMainInfoFlyout("Die Dateien wurden konvertiert.", false);
        }

        private void convertDOCtoPDFasync(string path, string fileName)
        {
            string rohdateiname = fileName.Substring(0, fileName.Length - 5);
            //MessageBox.Show(rohdateiname);
            try
            {
                if (!File.Exists(path + "\\" + rohdateiname + ".pdf"))
                {
                    if (File.Exists(path + "\\" + rohdateiname + ".docx"))
                    {
                        object misValue = System.Reflection.Missing.Value;
                        String PATH_APP_PDF = path + "\\" + rohdateiname + ".pdf";

                        var WORD = new Word.Application();

                        Word.Document doc = WORD.Documents.Open(path + "\\" + rohdateiname + ".docx");
                        doc.Activate();

                        doc.SaveAs2(@PATH_APP_PDF, Word.WdSaveFormat.wdFormatPDF, misValue, misValue, misValue,
                        misValue, misValue, misValue, misValue, misValue, misValue, misValue);

                        doc.Close();
                        WORD.Quit();

                        releaseObject(doc);
                        releaseObject(WORD);
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Die Konvertierung der Datei: " + fileName + " konnte nicht erfolgen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void releaseObject(object obj)
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

        private void GesamtListeErstellen(string path)
        {



            XFont font = new XFont("Verdana", 16);

            PdfDocument outputDocument = new PdfDocument();
            int counter = 0;
            PdfOutline outline = null;


            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (file.IndexOf(".pdf") >0 )
                {

                    PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                    // Iterate pages
                    string text = file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 5);

                    try
                    {
                        if (counter == 0)
                        {
                            PdfPage page = outputDocument.AddPage();
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            gfx.DrawString(text, font, XBrushes.Black, 90, 200, XStringFormats.Default);

                            // Create the root bookmark. You can set the style and the color.
                            outline = outputDocument.Outlines.Add("Voten", page, true, PdfOutlineStyle.Bold, XColors.Red);
                            outline.Outlines.Add(text, page, true);
                        }
                        else
                        {
                            PdfPage page = outputDocument.AddPage();
                            XGraphics gfx = XGraphics.FromPdfPage(page);

                            //string text = file.ToString();
                            gfx.DrawString(text, font, XBrushes.Black, 90, 200, XStringFormats.Default);

                            // Create a sub bookmark
                            outline.Outlines.Add(text, page, true);
                        }

                        int count = inputDocument.PageCount;
                        for (int idx = 0; idx < count; idx++)
                        {
                            // Get the page from the external document...
                            PdfPage page = inputDocument.Pages[idx];
                            // ...and add it to the output document.
                            outputDocument.AddPage(page);
                        }
                        counter++;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Folgende Datei konnte nicht hinzugefügt werden: " + file.ToString());
                    }
                }
                // Open the document to import pages from it.
            }
            // Iterate files
            // Save the document...
            string filename = path + "\\" + "Gesamtpdf.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            //Process.Start(filename);
            
        }
    }
}
