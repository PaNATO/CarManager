using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    /// Logika interakcji dla klasy CarAddForm.xaml
    /// </summary>
    public partial class CarAddForm : Window
    {
        public string UserID;
        public string UserIDTemp;
        readonly string connectionString = Properties.Settings.Default.ConnectionToDB;
        private void Func_Clear()
        {
            ModelInput.Clear();
            YearInput.Clear();
            MileageInput.Clear();
            FuelTankSize.Clear();
        }
        public class CarInfo
        {
            public string CarBrand { get; set; }
            public string CarType { get; set; }
            public string CarFuel { get; set; }
        }
        public CarAddForm()
        {
            InitializeComponent();
        // Otwarcie pliku oraz stworzenie listy pobierającej dane z pliku oraz wyświetlenie ich w ComboBox na ekranie
            string carBrandsfilePath = @"C:\Users\chief12\Source\Repos\Testy2\Testy2\CarData\CarBrands.txt";
            string carTypesfilePath = @"C:\Users\chief12\Source\Repos\Testy2\Testy2\CarData\CarTypes.txt";
            string carFuelfilePath = @"C:\Users\chief12\Source\Repos\Testy2\Testy2\CarData\CarFuel.txt";
            List<CarInfo> brands = new List<CarInfo>();
            List<string> carBrands = File.ReadAllLines(carBrandsfilePath).ToList();
            foreach (var brand in carBrands)
            {
                string[] brandData = brand.Split(';');
                CarInfo newCarBrand = new CarInfo
                {
                    CarBrand = brandData[0]
                };
                brands.Add(newCarBrand);

            }
            BrandSelect.ItemsSource = brands;
            BrandSelect.DisplayMemberPath = "CarBrand";
            List<CarInfo> brands2 = new List<CarInfo>();
            List<string> carTypes = File.ReadAllLines(carTypesfilePath).ToList();
            foreach (var type in carTypes)
            {
                string[] typesData = type.Split(';');
                CarInfo newCarType = new CarInfo
                {
                    CarType = typesData[0]
                };
                brands2.Add(newCarType);
            }
            TypeSelect.ItemsSource = brands2;
            TypeSelect.DisplayMemberPath = "CarType";
            List<CarInfo> brands3 = new List<CarInfo>();
            List<string> carFuel = File.ReadAllLines(carFuelfilePath).ToList();
            foreach (var fuel in carFuel)
            {
                string[] fuelData = fuel.Split(';');
                CarInfo newCarFuel = new CarInfo
                {
                    CarFuel = fuelData[0]
                };
                brands3.Add(newCarFuel);
            }
            FuelSelect.ItemsSource = brands3;
            FuelSelect.DisplayMemberPath = "CarFuel";
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (BrandSelect.SelectedItem is null && TypeSelect.SelectedItem is null && FuelSelect.SelectedItem is null ||
                BrandSelect.SelectedItem is null && TypeSelect.SelectedItem is null ||
                BrandSelect.SelectedItem is null && FuelSelect.SelectedItem is null ||
                TypeSelect.SelectedItem is null && FuelSelect.SelectedItem is null ||
                BrandSelect.SelectedItem is null || TypeSelect.SelectedItem is null || FuelSelect.SelectedItem is null ||
                BrandSelect.SelectedItem != null && TypeSelect.SelectedItem != null && FuelSelect.SelectedItem != null)
            {
                Func_Clear();
                BrandSelect.SelectedValue = BrandSelect.Items.ToString();
                TypeSelect.SelectedValue = TypeSelect.Items.ToString();
                FuelSelect.SelectedValue = FuelSelect.Items.ToString();
            }
            else
            {
                Func_Clear();
            }
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            string[] carInfo = new string[8];
            carInfo[0] = UserIDTemp;
            carInfo[1] = BrandSelect.Text;
            carInfo[2] = ModelInput.Text;
            carInfo[3] = TypeSelect.Text;
            carInfo[4] = YearInput.Text;
            carInfo[5] = MileageInput.Text;
            carInfo[6] = FuelSelect.Text;
            carInfo[7] = FuelTankSize.Text;

            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            string CarAddCommand = $"Insert into UserCars(UserID, CarBrand, Model, Type, Year, Mileage, Fuel, FuelTankSize)values('{carInfo[0]}','{carInfo[1]}','{carInfo[2]}','{carInfo[3]}','{carInfo[4]}','{carInfo[5]}','{carInfo[6]}','{carInfo[7]}')";
            if(ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand CarAdd = new SqlCommand(CarAddCommand, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                CarAdd.ExecuteNonQuery();
                ConnectionToDB.Close();
                //CarMenu carMenu = new CarMenu();
                //carMenu.Show();
                MessageBox.Show("Car added");
            }
            else
            {
                MessageBox.Show("Operation failed!");
            }
            this.Close();
        }

        public void UserIDBinding()
        {
             UserIDTemp = UserID;
        }
    }
}
