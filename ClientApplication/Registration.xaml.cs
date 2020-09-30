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
using SmartParkingApp;

namespace ClientApplication
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>C:\Users\Pavel Petrov\Desktop\Zhenya\ClientApplication\Registration.xaml.cs
    public partial class Registration : Window
    {
        ParkingManager parkingManager;
        public Registration(ParkingManager pm)
        {
            InitializeComponent();
            parkingManager = pm;
        }

        private void SingUp_Buttun_Click(object sender, RoutedEventArgs e)
        {
            var Login = Enter_Phone.Text;
            var Name = Enter_Name.Text;
            var CarPlateNumber = Enter_CarPlateNumber.Text;
            var Password = Enter_Password.Password;
            var RepeatPassword = Repeat_Password.Password;
            var BirthDate = Enter_BirthDate.SelectedDate;
            if (CheckRow(Login, Name, CarPlateNumber, Password, RepeatPassword, BirthDate))
            {
                if(parkingManager.Rewriter(Login, Name, BirthDate, Password, CarPlateNumber))
                {
                    Hide();
                    var window = new ClientWindow(parkingManager, Login);
                    window.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("This login is already existing, create another one");
                }

            }



        }





        public bool CheckRow(string Login, string Name, string CarPlateNumber, string Password, string RepeatPassword, DateTime? BirthDate)
        {
            if (Login != "" && Name != "" && CarPlateNumber != "" && Password != "" && RepeatPassword == Password && BirthDate != null)
                return true;
            else if (Login == "")
            {
                MessageBox.Show("Enter your login again, please");
                return false;
            }
            else if (Name == "")
            {
                MessageBox.Show("Enter your name again, please");
                return false;
            }
            else if (CarPlateNumber == "")
            {
                MessageBox.Show("Enter your car plate number again, please");
                return false;
            }
            else if (Password == "")
            {
                MessageBox.Show("Enter your password again, please");
                return false;
            }
            else if (RepeatPassword != Password)
            {
                MessageBox.Show("Passwods are not the same, enter your password again, please");
                return false;
            }
            else
            {
                MessageBox.Show("Enter your birht date again, please");
                return false;
            }

        }
    }
}
