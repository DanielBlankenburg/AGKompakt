using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.ActivityRequestClasses;
//using BGH_Kompakt.Migrations;
using BGH_Kompakt.Services;
using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BGH_Kompakt.Views
{
    /// <summary>
    /// Interaction logic for NebentaetigkeitenAnzeigeView.xaml
    /// </summary>
    public partial class NebentaetigkeitenAnzeigeView : UserControl
    {

        public NebentaetigkeitenAnzeigeViewModel NebentaetigkeitenAnzeigeViewModel { get; set; }

        public NebentaetigkeitenAnzeigeView(NebentaetigkeitenAnzeigeViewModel nebentaetigkeitenAnzeigeViewModel)
        {
            
            InitializeComponent();
            NebentaetigkeitenAnzeigeViewModel = nebentaetigkeitenAnzeigeViewModel;
            DataContext = NebentaetigkeitenAnzeigeViewModel;
            NebentaetigkeitenAnzeigeViewModel.OnClientClick += (s, e) => ClientClick(s);
            //NebentaetigkeitenAnzeigeViewModel vm = (NebentaetigkeitenAnzeigeViewModel)this.TryFindResource("vmNT");
        }

        private void ClientClick(object sender)
        {
            
        }
    }
}
