using BGH_Kompakt.Classes.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Dtos
{
    public class EMailResponse
    {
        public string EmailTo { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string EMailToCC { get; set; } = string.Empty;
        public string EMailToBCC { get; set; } = string.Empty;
        public List<CustomMailAttachment> Attachments { get; set; } = new List<CustomMailAttachment>();
    }
}
