using SmartParkingApp;
using SmartParkingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientApplication
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        ParkingManager parkingManager;
        User user;
        public ClientWindow(ParkingManager pm, string login)
        {
            InitializeComponent();
            parkingManager = pm;
            user = pm.User_Id(login);
            GreetingText.Text = " Hello, " + user.Name;
            User_ID.Text = $" Phone  {user.Login}";
            Car_ID.Text =" Car " + user.CarPlateNumber;


        }

        private void ActiveSessionButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var actWin = new ActiveSession(parkingManager, user);
            if (actWin.CarNumber.Text != "The Car Number")
                actWin.ShowDialog();
            Show();

        }

        private void PassiveSessionButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var Passive_Session = new PassiveSession(parkingManager, user);
            Passive_Session.Show();
            Close();

        }
    }
}
