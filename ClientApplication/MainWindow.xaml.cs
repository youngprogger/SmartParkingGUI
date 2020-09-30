using SmartParkingApp;
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

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        ParkingManager parkingManager;
        public MainWindow()
        {
            parkingManager = new ParkingManager();
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (parkingManager.CheckLoginAndPassword(Login.Text, Password.Password))
            {
                Hide();
                var Enter = new ClientWindow(parkingManager, Login.Text);
                Enter.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Login or password is wrong, enter your login and passwors, please");
            }

            
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var Reg = new Registration(parkingManager);
            Reg.Show();
            Close();
        }
        
    }
}
