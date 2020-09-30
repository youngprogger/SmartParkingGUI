using SmartParkingApp;
using SmartParkingApp.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {  
        ParkingManager pm;
        public MainWindow()
        {
            InitializeComponent();
            pm  = new ParkingManager();
        }
        private void Past_Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var past = new PassiveSession(pm);
            past.Show();
            Close();
            
        }
        private void Active_Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var active = new ActiveSession(pm);
            active.Show();
            Close();
        }
    }
}
