using BGH_Kompakt.Classes;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BGH_Kompakt.Views
{
    /// <summary>
    /// Interaktionslogik für Kanzleiunterlagen.xaml
    /// </summary>
    public partial class Kanzleiunterlagen : UserControl
    {
        private ObservableCollection<ComboboxItems> regZ = new ObservableCollection<ComboboxItems>();
        public ListCollectionView regZlist;
        public Kanzleiunterlagen()
        {
            InitializeComponent();
            Cbo_RegZ_Fill();
            regZlist = new ListCollectionView(regZ);
            Cbo_RegZ.ItemsSource = regZlist;
        }

        public void Cbo_RegZ_Fill()
        {
            regZ.Add(new ComboboxItems(0, "ZR"));
            regZ.Add(new ComboboxItems(1, "ZB"));
            regZ.Add(new ComboboxItems(2, "ZA"));
        }

        private void Cbo_RegZ_DropDownClosed(object sender, EventArgs e)
        {
            int num = (int)MessageBox.Show("\\\\bgh.bund.de\\dfs\\Kanzlei\\Senat9\\IX" + Cbo_RegZ.Text);
        }
    }
}
