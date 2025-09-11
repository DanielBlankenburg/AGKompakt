using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.UserService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace BGH_Kompakt.Views.UserLogin
{
    /// <summary>
    /// Interaction logic for UserSwitchWindow.xaml
    /// </summary>
    public partial class UserSwitchWindow : Window
    {
        public List<User> UserList { get; set; }
        public UserSwitchWindow()
        {
            InitializeComponent();
            UserList = new List<User>();
            UserDBContext userDBContext = new UserDBContext();
            var query = userDBContext.Users;
            foreach (var user in query) UserList.Add(user);
            Lst_User.ItemsSource = UserList;
        }

        private void Btn_Change_Click(object sender, RoutedEventArgs e)
        {
            User SelectedUser = new User();
            foreach (User selectedItem in (IEnumerable)Lst_User.SelectedItems) SelectedUser = selectedItem;

            UserDBContext userDBContext = new UserDBContext();
            SelectedUser = userDBContext.Users.Where(u => u.UserId == SelectedUser.UserId).Include(x => x.AdminStatus).FirstOrDefault();
            UserManager.RegistratedUser = SelectedUser;
            this.Close();
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
