using BGH_Kompakt.Classes.Test;
using BGH_Kompakt.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel
{
    public partial class TestViewModel : ViewModelBase
    {
        public ObservableCollection<ItemPresenter> Items { get; }
           = new ObservableCollection<ItemPresenter>();

        public ItemPresenter SelectedItem { get; set; }
        public int SelectedIndex { get; set; } = 0;
       //    {
       // new ItemPresenter("A"),
       // new ItemPresenter("B"),
       // new ItemPresenter("C"),
       // new ItemPresenter("D")
       //};

        public TestViewModel()
        {
            Items.Add(new ItemPresenter("A-Eintrag"));
            Items.Add(new ItemPresenter("B-Eintrag"));
            Items.Add(new ItemPresenter("C-Eintrag"));
            Items.Add(new ItemPresenter("D-Eintrag"));

            SelectedItem = Items.Where(x => x.Ausgabetext == "D-Eintrag").FirstOrDefault();
            SelectedIndex = 2;
        }

        public ICommand SelectAllCommand => new RelayCommand(_ =>
        {
            //foreach (var item in Items)
            //{
            //    item.IsSelected = true;
            //}
            //var itemlist = Items.Where(i => i.IsSelected);
            //foreach (var item in itemlist)
            //{
            //    MessageBox.Show(item.ToString());
            //}

            MessageBox.Show(SelectedItem.ToString());

            
        });
    }
}
