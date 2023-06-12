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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLocateMe_Click(object sender, RoutedEventArgs e)
        {
            APICall.LocateMe();
        }

        private void btnSearchCity_Click(object sender, RoutedEventArgs e)
        {
            APICall.SearchMyCity(txtBoxCity.Text, txtBoxCountry.Text);
        }
    }
}
