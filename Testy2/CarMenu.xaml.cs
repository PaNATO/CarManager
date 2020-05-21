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
            UserCarsListFunc();
        }

        readonly string connectionString = Properties.Settings.Default.ConnectionToDB;
        public string UName;
        public string UserID;
        class UserCars
        {
            public string CarBrand { get; set; }
            public string Model { get; set; }
            public string Type { get; set; }
            public string Year { get; set; }
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            CarAddForm NewCar = new CarAddForm();
            NewCar.Show();
            NewCar.UserID = UserID;
            NewCar.Binding();
            this.Hide();
        }
        public void Wyświetl()
        {
            UNameShow.Content = UName+" "+UserID;
        }
        
        public void UserCarsListFunc()
        {
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            String AllUserCars = "Select * from UserCars";
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
                            CarBrand = UserCarsReader.GetString(2),
                            Model = UserCarsReader.GetString(3),
                            Type = UserCarsReader.GetString(4),
                            Year = UserCarsReader[5].ToString()
                        };

                        UserCarsList.ItemsSource = UserCarsLi;
                        UserCarsLi.Add(UserCarsProp);
                    }
                }ConnectionToDB.Close();
            }

        }
    }
}
