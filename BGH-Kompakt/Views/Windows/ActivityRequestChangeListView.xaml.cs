using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BGH_Kompakt.Views.Windows
{
    /// <summary>
    /// Interaction logic for ActivityRequestChangeListView.xaml
    /// </summary>
    public partial class ActivityRequestChangeListView : Window
    {
        private ActivityRequest ActivityRequest { get; set; }
        public ActivityRequestChangeListView(ActivityRequest activityRequest)
        {
            ActivityRequest = activityRequest;
            InitializeComponent();
            if (ActivityRequest != null && ActivityRequest.ActivityRequestChangeHistories != null) this.ListChanges.ItemsSource = ActivityRequest.ActivityRequestChangeHistories;
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
