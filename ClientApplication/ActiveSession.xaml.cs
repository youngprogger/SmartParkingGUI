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
    /// Логика взаимодействия для ActiveSession.xaml
    /// </summary>
    public partial class ActiveSession : Window
    {
        ParkingManager parkingManager;
        User user;
        ParkingSession active;
        public ActiveSession(ParkingManager pm, User user)
        {
            InitializeComponent();
            parkingManager = pm;
            this.user = user;
            var active = pm.Active_Session(user);
            if (active == null)
            {
                MessageBox.Show("Acrive session does not exist");
                Close();
            }
            else
            {
                Time.Text = (DateTime.Now - active.EntryDt).ToString();
                TicketNumber.Text = " Ticket - " + active.TicketNumber;
                CarNumber.Text = " Car - " + active.CarPlateNumber;
            }

        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var active = new ActiveSession(parkingManager, user);
            active.Show();
            Close();

        }

        private void GetCost_Button_Click(object sender, RoutedEventArgs e)
        {
            parkingManager.GetRemainingCost(active.TicketNumber);
        }
    }
}
