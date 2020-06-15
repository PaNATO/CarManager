using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Testy2
{
    /// <summary>
    /// Logika interakcji dla klasy CarMenu.xaml
    /// </summary>
    public partial class CarMenu : Window
    {
        public CarMenu()
        {
            InitializeComponent();
        }

        readonly string connectionString = Properties.Settings.Default.ConnectionToDB;
        public string UName;
        public string UserID;
        class UserCars
        {
            public string CarID { get; set; }
            public string CarBrand { get; set; }
            public string Model { get; set; }
            public string Type { get; set; }
            public string Year { get; set; }
            public string Mileage { get; set; }
            public string Fuel { get; set; }
            public string FuelTankSize { get; set; }
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            CarAddForm NewCar = new CarAddForm
            {
                UserID = UserID
            };
            NewCar.UserIDBinding();
            NewCar.ShowDialog();
            

        }
        public void Wyświetl()
        {
            UNameShow.Content = UName;
        }

        public void UserCarsListFunc()
        {
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            String AllUserCars = "Select * from UserCars where UserID = '" + UserID + "'";
            if(ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand UserCarsSelect = new SqlCommand(AllUserCars, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader UserCarsReader;
                UserCarsReader = UserCarsSelect.ExecuteReader();
                List<UserCars> UserCarsLi = new List<UserCars>();
                while (UserCarsReader.Read())
                {
                    if (!UserCarsReader.IsDBNull(0))
                    {
                        UserCars UserCarsProp = new UserCars
                        {
                            CarID = UserCarsReader[0].ToString(),
                            CarBrand = UserCarsReader.GetString(2),
                            Model = UserCarsReader.GetString(3),
                            Type = UserCarsReader.GetString(4),
                            Year = UserCarsReader[5].ToString(),
                            Mileage = UserCarsReader[6].ToString(),
                            Fuel = UserCarsReader.GetString(7),
                            FuelTankSize = UserCarsReader[8].ToString()
                        };

                        UserCarsList.ItemsSource = UserCarsLi;
                        UserCarsLi.Add(UserCarsProp);
                    }
                }ConnectionToDB.Close();
            }
        }

        private void RefreshCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserCarsList.ItemsSource != null)
            {
                UserCarsList.ItemsSource = null;
            }else
            UserCarsListFunc();
        }
        public void UserCarsDataBinding()
        {
            UserCarMain UserCarMain = new UserCarMain();
            this.Close();
            var userCarsBinding = ((Testy2.CarMenu.UserCars)UserCarsList.SelectedValue);
            UserCarMain.CarID = userCarsBinding.CarID;
            UserCarMain.Brand = userCarsBinding.CarBrand;
            UserCarMain.Model = userCarsBinding.Model;
            UserCarMain.Type = userCarsBinding.Type;
            UserCarMain.Year = userCarsBinding.Year;
            UserCarMain.Mileage = userCarsBinding.Mileage;
            UserCarMain.Fuel = userCarsBinding.Fuel;
            UserCarMain.FuelTankSize = userCarsBinding.FuelTankSize;
            UserCarMain.UserName = UName;
            UserCarMain.UserID = UserID;
            UserCarMain.CarShow();
            UserCarMain.PeriodicInspectionSelectData();
            UserCarMain.Show();
        }
        private void UserCarList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserCarsDataBinding();
        }
    }
}