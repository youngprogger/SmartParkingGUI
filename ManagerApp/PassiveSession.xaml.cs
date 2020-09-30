using SmartParkingApp;
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

namespace ManagerApp
{
    /// <summary>
    /// Логика взаимодействия для PassiveSession.xaml
    /// </summary>
    public partial class PassiveSession : Window
    {
        ParkingManager pm;
        public PassiveSession(ParkingManager pm)
        {
            InitializeComponent();
            this.pm = pm;
            Passive_Sessions.ItemsSource = pm.Passive_Sessions();
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var passive = new PassiveSession(pm);
            passive.Show();
            Close();
        }

        private void Passive_Sessions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dateString = Passive_Sessions.SelectedItem.ToString().Split(';');
            var EntryDt = dateString[1];
            var ExitDT = dateString[2];
            var Find = pm.Find_String(EntryDt, ExitDT);
            Date.Text = "Entry Date Time - " + Find.EntryDt.ToString();
            Parking_Time.Text = "Parking Time - " + (Find.ExitDt.Value - Find.EntryDt).TotalMinutes.ToString();
            Car_Plate.Text = " Car Plate - " + Find.CarPlateNumber;
            Cost.Text = " Cost - " + Find.TotalPayment.ToString();
        }
    }
}
