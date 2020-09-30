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
    /// Логика взаимодействия для ActiveSession.xaml
    /// </summary>
    public partial class ActiveSession : Window
    {
        ParkingManager pm;
        public ActiveSession(ParkingManager pm)
        {
            InitializeComponent();
            this.pm = pm;
            Active_Sessions.ItemsSource = pm.Active_Sessions();
            Progress_Bar.Value = pm.ProgressBar() * 100;
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var active = new ActiveSession(pm);
            active.Show();
            Close();
        }

        private void Active_Sessions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dateString = Active_Sessions.SelectedItem.ToString().Split(';');
            var EntryDt = dateString[1];
            var ExitDt = dateString[2];
            var Find = pm.Find_String(EntryDt, ExitDt);
            Date.Text = "Entry Date Time - " + Find.EntryDt.ToString();
            Parking_Time.Text = "Parking Time - " + (DateTime.Now - Find.EntryDt).TotalMinutes.ToString();
            Car_Plate.Text = " Car Plate - " + Find.CarPlateNumber;
            Cost.Text = " Cost - " + Find.TotalPayment.ToString();
        }
    }
}
