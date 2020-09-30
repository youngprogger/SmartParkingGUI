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
    /// Логика взаимодействия для PassiveSession.xaml
    /// </summary>
    public partial class PassiveSession : Window
    {
        ParkingManager pm;
        User user;
        public PassiveSession(ParkingManager pm, User user)

        {
            InitializeComponent();
            this.pm = pm;
            this.user = user;
            Passive_Sessions.ItemsSource = pm.Passive_Session(user);

        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var passive = new PassiveSession(pm, user);
            passive.Show();
            Close();


            
        }

        private void Passive_Sessions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dateString = Passive_Sessions.SelectedItem.ToString().Split(';');
            var EntryDt = dateString[1];
            var ExitDT = dateString[2];
            var Find =  pm.Find_String(EntryDt, ExitDT);
            Date.Text = "Entry Date Time - " + Find.EntryDt.ToString();
            Parking_Time.Text = "Parking Time - " + (Find.ExitDt.Value - Find.EntryDt).TotalMinutes.ToString();
            Car_Plate.Text = " Car Plate - " + user.CarPlateNumber;
            Cost.Text = " Cost - " + Find.TotalPayment.ToString();
        }
    }
}
