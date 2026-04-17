using BGH_Kompakt.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BGH_Kompakt.Views.Windows
{
    /// <summary>
    /// Interaction logic for PreviewEMail.xaml
    /// </summary>
    public partial class PreviewEMail : Window
    {
        public PreviewEMail(EMailResponse eMail)
        {
            InitializeComponent();
            if(eMail != null)
            {
                Text_EMailTo.Text = eMail.EmailTo;
                Text_EMailToBCC.Text = eMail.EMailToBCC;
                Text_Subject.Text = eMail.Subject;
                Text_Body.Text = FormatBody(eMail.Body);
            }
        }

        private string FormatBody(string body)
        {
            string text = Regex.Replace(body, "<br>", "\n");
            return text;
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e) => Close();

    }
}
