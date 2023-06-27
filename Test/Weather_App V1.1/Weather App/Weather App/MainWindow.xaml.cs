using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Weather_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static DBAccess DB_Access = new DBAccess();

        public MainWindow()
        {
            InitializeComponent();

            APICall.LocateMe();
        }

        private void btnLocateMe_Click(object sender, RoutedEventArgs e)
        {
            APICall.LocateMe();
        }

        private void txtBoxCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtBoxCity.Text != "")
            {
                ListBoxCitites.Visibility = Visibility.Visible;
            }
            else
            {
                ListBoxCitites.Visibility = Visibility.Collapsed;
            }

            ListBoxCitites.ItemsSource = DB_Access.listCities(txtBoxCity.Text);         
        }

        private void listBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ListBoxCitites.SelectedIndex != -1)
            {
                string selectedCity = ListBoxCitites.SelectedItem.ToString();
                APICall.SearchMyCity(selectedCity.Split(", ")[0], selectedCity.Split(", ")[1]);
            }
            ListBoxCitites.UnselectAll();
            txtBoxCity.Text = "";

        }
    }
}
