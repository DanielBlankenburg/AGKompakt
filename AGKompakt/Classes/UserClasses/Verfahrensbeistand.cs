using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BGH_Kompakt.Classes.UserClasses
{
    public class Verfahrensbeistand : Person
    {   
        public int VerfahrensbeistandId { get; set; }
        public DateTime LastBZR { get; set; }
        public IList<Sprache> Sprachen { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public int PLZ { get; set; }
        public string Stadt { get; set; }
        public string Telefon { get; set; }
        public string Mobiltelefon { get; set; }


        // Image storage
        public byte[] Photo { get; set; }
        public string PhotoFileName { get; set; }
        public string PhotoContentType { get; set; }

        /// <summary>
        /// Synchronously set photo from a file path (reads bytes into Photo).
        /// </summary>
        public void SetPhotoFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Photo file not found", filePath);

            Photo = File.ReadAllBytes(filePath);
            PhotoFileName = Path.GetFileName(filePath);
            PhotoContentType = GetMimeTypeFromExtension(Path.GetExtension(filePath));
        }

        /// <summary>
        /// Async variant to set photo from disk.
        /// </summary>
        public async Task SetPhotoFromFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Photo file not found", filePath);

            Photo = await Task.Run(() => File.ReadAllBytes(filePath)).ConfigureAwait(false);
            PhotoFileName = Path.GetFileName(filePath);
            PhotoContentType = GetMimeTypeFromExtension(Path.GetExtension(filePath));
        }

        private static string GetMimeTypeFromExtension(string ext)
        {
            if (string.IsNullOrEmpty(ext)) return "application/octet-stream";
            ext = ext.ToLowerInvariant().TrimStart('.');
            switch (ext)
            {
                case "jpg":
                case "jpeg": return "image/jpeg";
                case "png": return "image/png";
                case "gif": return "image/gif";
                case "bmp": return "image/bmp";
                case "tiff":
                case "tif": return "image/tiff";
                default: return "application/octet-stream";
            }
        }

        [NotMapped]
        public BitmapImage PhotoImage
        {
            get
            {
                if (Photo == null || Photo.Length == 0) return null;
                try
                {
                    using (var ms = new MemoryStream(Photo))
                    {
                        var img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.StreamSource = ms;
                        img.EndInit();
                        img.Freeze();
                        return img;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
