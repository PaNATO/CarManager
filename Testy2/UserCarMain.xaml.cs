using Microsoft.Win32;
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
using System.Windows.Shapes;
using IronOcr;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace Testy2
{
    /// <summary>
    /// Logika interakcji dla klasy UserCarMain.xaml
    /// </summary>
    public partial class UserCarMain : Window
    {
        public UserCarMain()
        {
            InitializeComponent();
            AddRepairLogListFunc();
            AddRefuelLogListFunc();
            StatisticListFunc();
        }
        class Repairs
        {
            public string RepairDate { get; set; }
            public string RepairMileage { get; set; }
            public string RepairChange { get; set; }
            public string RepairPaid { get; set; }
        }

        class Refuels
        {
            public string TraveledDistanceTank { get; set; }
            public string RefuelMileage { get; set; }
            public string RefuelFuelType { get; set; }
            public string RefuelLiters { get; set; }
            public string RefuelPricePerLiter { get; set; }
            public string RefuelPaid { get; set; }
            public string RefuelDate { get; set; }
        }
        class Statistic
        {
            public string RepairCosts { get; set; }
            public string TraveledKilometers { get; set; }
            public string FirstRepairDate { get; set; }
            public string BestTankDistance { get; set; }
            public string PricyRefuel { get; set; }
            public string LifetimeCombustion { get; set; }
            public string BurnedFuel { get; set; }
            public string PaidForFuel { get; set; }
            public string AllCarCosts { get; set; }
        }

        public string CarID, Brand, Model, Type, Year, Mileage, Fuel, FuelTankSize;
        public string UserName;
        public string UserID;
        readonly string connectionString = Properties.Settings.Default.ConnectionToDB;
        public bool thereisdata;
        readonly string[] periodicinspection = new string[8];
        readonly string[] addrepairlog = new string[5];
        readonly string[] addrefuellog = new string[8];
        public void CarShow()
        {
            UUsernameCar.Content = "Username: " + UserName;
            UCurrentCar.Content = "Current car: " + Brand + " " + Model + " " + Type + " " + Year + " " + Fuel;
        }

        #region Methods for Periodic Inspection
        public void PeriodicInspectionDataBinding()
        {
            periodicinspection[0] = ActualInspectionDate.Text = Convert.ToDateTime(ActualInspectionDate.Text).ToString("yyyy-MM-dd");
            periodicinspection[1] = ActualMileageTextBox.Text;
            periodicinspection[2] = ActualOilFilterDate.Text = Convert.ToDateTime(ActualInspectionDate.Text).ToString("yyyy-MM-dd"); ;
            periodicinspection[3] = ActualAirFilterDate.Text = Convert.ToDateTime(ActualInspectionDate.Text).ToString("yyyy-MM-dd"); ;
            periodicinspection[4] = ActualFuelFilterDate.Text = Convert.ToDateTime(ActualInspectionDate.Text).ToString("yyyy-MM-dd"); ;
            periodicinspection[5] = ActualCabinFilterDate.Text = Convert.ToDateTime(ActualInspectionDate.Text).ToString("yyyy-MM-dd"); ;
            periodicinspection[6] = ActualUsedOilTextBox.Text;
            periodicinspection[7] = CarID;
        }

        public void PeriodicInspectionUpdatetData()
        {
            PeriodicInspectionDataBinding();
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            string PeriodicInspectionUpdateCommand = $"Update UserCarsPeriodicInspection " +
                $"set InspectionDate = '"+ periodicinspection[0]+"'," +
                "InspectionMileage = '" + periodicinspection[1] + "'," +
                "OilFilterDate = '" + periodicinspection[2] + "'," +
                "AirFilterDate = '" + periodicinspection[3] + "'," +
                "FuelFilterDate = '" + periodicinspection[4] + "'," +
                "CabinFilterDate = '" + periodicinspection[5] + "'," +
                "OilType = '" + periodicinspection[6] + "'" +
                "where InspectionCarID ='" + CarID + "'";
            if(ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand PeriodicInspectionUpdate = new SqlCommand(PeriodicInspectionUpdateCommand, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                PeriodicInspectionUpdate.ExecuteNonQuery();
                ConnectionToDB.Close();
            }
        }

        public void PeriodicInspectionSelectData()
        {
            
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            string PeriodicInspectionSelectCommand = "Select * from UserCarsPeriodicInspection where InspectionCarID ='" + CarID + "'";
            if (ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand PeriodicInspectionSelect = new SqlCommand(PeriodicInspectionSelectCommand, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader InspectionReader;
                InspectionReader = PeriodicInspectionSelect.ExecuteReader();
                while (InspectionReader.Read())
                {
                    if (!InspectionReader.IsDBNull(0))
                    {
                        ActualInspectionDate.Text = InspectionReader[1].ToString();
                        ActualMileageTextBox.Text = InspectionReader[2].ToString();
                        ActualOilFilterDate.Text = InspectionReader[3].ToString();
                        ActualAirFilterDate.Text = InspectionReader[4].ToString();
                        ActualFuelFilterDate.Text = InspectionReader[5].ToString();
                        ActualCabinFilterDate.Text = InspectionReader[6].ToString();
                        ActualUsedOilTextBox.Text = InspectionReader[7].ToString();
                        thereisdata = true;
                    }else
                        thereisdata = false;
                }
                ConnectionToDB.Close();
            }
        }
        #endregion
        #region Methods for RepairLog
        public void AddRepairLogDataBinding()
        {
            addrepairlog[0] = AddRepairDoneDate.Text = Convert.ToDateTime(AddRepairDoneDate.Text).ToString("yyyy-MM-dd");
            addrepairlog[1] = AddRepairDoneMileageTextBox.Text;
            addrepairlog[2] = AddRepairChangeTextBox.Text;
            addrepairlog[3] = AddRepairCostTextBox.Text;
            addrepairlog[4] = CarID;
        }
        public void AddRepairLogDataClear()
        {
            AddRepairDoneDate.Text = AddRepairDoneMileageTextBox.Text = AddRepairChangeTextBox.Text = AddRepairCostTextBox.Text = null;
        }
        public void AddRepairLogInsertData()
        {
            AddRepairLogDataBinding();
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            string AddRepairInsertCommand = $"Insert into UserCarsRepairLog" +
                $"(RepairDate, RepairMileage, RepairChange, RepairPaid, RepairCarID) " +
                $"values(" +
                $"'{addrepairlog[0]}'," +
                $"'{addrepairlog[1]}'," +
                $"'{addrepairlog[2]}'," +
                $"'{addrepairlog[3]}'," +
                $"'{addrepairlog[4]}')";
            if(ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand AddRepairInsert = new SqlCommand(AddRepairInsertCommand, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                AddRepairInsert.ExecuteNonQuery();
                ConnectionToDB.Close();
                MessageBox.Show("Repair added");
            }
            else
            {
                MessageBox.Show("Error corupted Data");
            }
        }

        public void AddRepairLogListFunc()
        {
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            String AllUserCarsRepairs = "Select * from UserCarsRepairLog where RepairCarID ='" + CarID + "' order by RepairDate desc";
            if (ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand UserCarsSelect = new SqlCommand(AllUserCarsRepairs, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader UserCarsRepairsReader;
                UserCarsRepairsReader = UserCarsSelect.ExecuteReader();
                List<Repairs> UserCarsRepairList = new List<Repairs>();
                while (UserCarsRepairsReader.Read())
                {
                    if (!UserCarsRepairsReader.IsDBNull(0))
                    {
                        Repairs UserCarsProp = new Repairs
                        {
                            RepairDate = UserCarsRepairsReader[1].ToString().Substring(0,10),
                            RepairMileage = UserCarsRepairsReader[2].ToString() +" km",
                            RepairChange = UserCarsRepairsReader[3].ToString(),
                            RepairPaid = UserCarsRepairsReader[4].ToString() +" pln",
                        };

                        UserCarRepairsListBox.ItemsSource = UserCarsRepairList;
                        UserCarsRepairList.Add(UserCarsProp);
                    }
                }
                ConnectionToDB.Close();
            }
        }
        #endregion
        #region Methods for RefuelLog
        public void AddRefuelLogDataBinding()
        {
            addrefuellog[0] = AddRefuelTraveledDistanceTextBox.Text = Convert.ToDecimal(AddRefuelTraveledDistanceTextBox.Text, CultureInfo.CurrentCulture).ToString("0,0");
            addrefuellog[1] = AddRefuelMileageTextBox.Text;
            addrefuellog[2] = AddRefuelFuelTypeTextBox.Text;
            addrefuellog[3] = AddRefuelFuelAmountTextBox.Text = Convert.ToDecimal(AddRefuelFuelAmountTextBox.Text, CultureInfo.CurrentCulture).ToString("0,0");
            addrefuellog[4] = AddRefuelFuelPriceTextBox.Text = Convert.ToDecimal(AddRefuelFuelPriceTextBox.Text, CultureInfo.CurrentCulture).ToString("0");
            addrefuellog[5] = AddRefuelPriceTextBox.Text = Convert.ToDecimal(AddRefuelPriceTextBox.Text, CultureInfo.CurrentCulture).ToString("0,0");
            addrefuellog[6] = AddRefuelDateDatePicker.Text = Convert.ToDateTime(AddRefuelDateDatePicker.Text).ToString("yyyy-MM-dd");
            addrefuellog[7] = CarID;
        }
        public void AddRefuelLogDataClear()
        {
            AddRefuelTraveledDistanceTextBox.Text = AddRefuelMileageTextBox.Text = AddRefuelFuelTypeTextBox.Text = AddRefuelFuelAmountTextBox.Text = AddRefuelFuelPriceTextBox.Text = AddRefuelPriceTextBox.Text = AddRefuelDateDatePicker.Text = null;
        }
        public void AddRefuelLogInsertData()
        {
            AddRefuelLogDataBinding();
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            string AddRepairInsertCommand = $"Insert into UserCarsRefuelLog" +
                $"(TraveledDistanceTank, RefuelMileage, RefuelFuelType, RefuelLiters, RefuelPricePerLiter, RefuelPaid, RefuelDate, RefuelCarID) " +
                $"values(" +
                $"'{Convert.ToDecimal(addrefuellog[0], System.Globalization.CultureInfo.CurrentCulture)}'," +
                $"'{addrefuellog[1]}'," +
                $"'{addrefuellog[2]}'," +
                $"'{Convert.ToDecimal(addrefuellog[3], System.Globalization.CultureInfo.CurrentCulture)}'," +
                $"'{Convert.ToDecimal(addrefuellog[4], System.Globalization.CultureInfo.CurrentCulture)}'," +
                $"'{Convert.ToDecimal(addrefuellog[5], System.Globalization.CultureInfo.CurrentCulture)}'," +
                $"'{addrefuellog[6]}'," +
                $"'{addrefuellog[7]}')";
            if (ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand AddRefuelInsert = new SqlCommand(AddRepairInsertCommand, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                AddRefuelInsert.ExecuteNonQuery();
                ConnectionToDB.Close();
                MessageBox.Show("Refuel added");
            }
            else
            {
                MessageBox.Show("Error corupted Data");
            }
        }

        public void AddRefuelLogListFunc()
        {
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            String AllUserCarsRefuels = "Select * from UserCarsRefuelLog where RefuelCarID ='" + CarID + "' order by RefuelDate desc";
            if (ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand UserCarsSelect = new SqlCommand(AllUserCarsRefuels, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader UserCarsRefuelReader;
                UserCarsRefuelReader = UserCarsSelect.ExecuteReader();
                List<Refuels> UserCarsRefuelList = new List<Refuels>();
                while (UserCarsRefuelReader.Read())
                {
                    if (!UserCarsRefuelReader.IsDBNull(0))
                    {
                        Refuels UserCarsProp = new Refuels
                        {
                            TraveledDistanceTank = UserCarsRefuelReader[1].ToString() + " km",
                            RefuelMileage = UserCarsRefuelReader[2].ToString() + " km",
                            RefuelFuelType = UserCarsRefuelReader[3].ToString(),
                            RefuelLiters = UserCarsRefuelReader[4].ToString() + " L",
                            RefuelPricePerLiter = UserCarsRefuelReader[5].ToString()+ " pln/L",
                            RefuelPaid = UserCarsRefuelReader[6].ToString() + " pln",
                            RefuelDate = UserCarsRefuelReader[7].ToString().Substring(0,10),
                        };

                        UserCarRefuelLogListBox.ItemsSource = UserCarsRefuelList;
                        UserCarsRefuelList.Add(UserCarsProp);
                    }
                }
                ConnectionToDB.Close();
            }
        }
        #endregion
        #region Methods for Statistic
        public void StatisticListFunc()
        {
            SqlConnection ConnectionToDB = new SqlConnection(connectionString);
            String AllUserCarsStatistic = "select sum(RepairPaid) as RepairCosts," +
                "(select max(RepairMileage) - min(RepairMileage) from UserCarsRepairLog where RepairCarID = '"+ CarID +"') as TraveledKilometers," +
                "(select min(RepairDate) from UserCarsRepairLog where RepairCarID = '" + CarID + "')as FirstRepairDate," +
                "(select max(TraveledDistanceTank) from UserCarsRefuelLog where RefuelCarID = '" + CarID + "')as BestTankDistance," +
                "(select max(RefuelPaid) from UserCarsRefuelLog where RefuelCarID = '" + CarID + "')as PricyRefuel," +
                "(select(sum(RefuelLiters) / sum(TraveledDistanceTank)) * 100  from UserCarsRefuelLog where RefuelCarID = '" + CarID + "')as LifetimeCombustion," +
                "(select sum(RefuelLiters) from UserCarsRefuelLog where RefuelCarID = '" + CarID + "')as BurnedFuel," +
                "(select sum(RefuelPaid) from UserCarsRefuelLog where RefuelCarID = '" + CarID + "')as PaidForFuel," +
                "(select sum(RepairPaid) + (select sum(RefuelPaid) from UserCarsRefuelLog) from UserCarsRepairLog where RepairCarID = '" + CarID + "')as AllCarCosts from UserCarsRepairLog where RepairCarID = '" + CarID + "' ; ";
            if (ConnectionToDB.State != ConnectionState.Open)
            {
                ConnectionToDB.Open();
                SqlCommand UserCarsSelect = new SqlCommand(AllUserCarsStatistic, ConnectionToDB)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader UserCarsRefuelStatistic;
                UserCarsRefuelStatistic = UserCarsSelect.ExecuteReader();
                List<Statistic> UserCarsStatistic = new List<Statistic>();
                while (UserCarsRefuelStatistic.Read())
                {
                    if (!UserCarsRefuelStatistic.IsDBNull(0))
                    {
                        Statistic UserCarsProp = new Statistic
                        {
                            RepairCosts = "Repair Costs: -- " + UserCarsRefuelStatistic[0].ToString() + " pln",
                            TraveledKilometers = "Traveled Kilometers: -- " + UserCarsRefuelStatistic[1].ToString() + " km",
                            FirstRepairDate = "First Repair Date: -- " + UserCarsRefuelStatistic[2].ToString().Substring(0, 10),
                            BestTankDistance = "Best Tank Distance: -- " + UserCarsRefuelStatistic[3].ToString() + " km",
                            PricyRefuel = "The Most Pricy Refuel: -- " + UserCarsRefuelStatistic[4].ToString() + " pln",
                            LifetimeCombustion = "Lifetime Combustion: -- " + UserCarsRefuelStatistic[5].ToString() + " L",
                            BurnedFuel = "All Burned Fuel: -- " + UserCarsRefuelStatistic[6].ToString() + " L",
                            PaidForFuel = "All Fuel Costs: -- " + UserCarsRefuelStatistic[7].ToString() + " pln",
                            AllCarCosts = "All Car Costs: -- " + UserCarsRefuelStatistic[8].ToString() + " pln",
                        };

                        UserCarsStatisticListBox.ItemsSource = UserCarsStatistic;
                        UserCarsStatistic.Add(UserCarsProp);
                    }
                }
                ConnectionToDB.Close();
            }
        }
        #endregion
        #region Actions
        private void UserCarsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserCarRepairsListBox.ItemsSource = null;
            AddRepairLogListFunc();
            UserCarRefuelLogListBox.ItemsSource = null;
            AddRefuelLogListFunc();
            UserCarsStatisticListBox.ItemsSource = null;
            StatisticListFunc();
        }

        private void UserCarChangeCarButton_Click(object sender, RoutedEventArgs e)
        {
            CarMenu CarMenu = new CarMenu
            {
                UName = UserName,
                UserID = UserID
            };
            CarMenu.Wyświetl();
            CarMenu.UserCarsListFunc();
            this.Close();
            CarMenu.Show();
        }

        private void UserCarLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogReg LogReg = new LogReg();
            LogReg.Show();
            this.Close();
        }

        private void AddRefuelDataSaveButton_Click(object sender, RoutedEventArgs e)
        {
            AddRefuelLogInsertData();
            AddRefuelLogDataClear();
        }
        private void AddRepairSaveButton_Click(object sender, RoutedEventArgs e)
        {
            AddRepairLogInsertData();
            AddRepairLogDataClear();
        }
        private void PeriodicInspectionSaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            if(thereisdata == false)
            {
                PeriodicInspectionDataBinding();
                SqlConnection ConnectionToDB = new SqlConnection(connectionString);
                string PeriodicInspectionAddCommand = $"Insert into UserCarsPeriodicInspection" +
                    $"(InspectionDate, InspectionMileage, OilFilterDate, AirFilterDate, FuelFilterDate, CabinFilterDate, OilType, InspectionCarID)" +
                    $"values(" +
                    $"'{periodicinspection[0]}'," +
                    $"'{periodicinspection[1]}'," +
                    $"'{periodicinspection[2]}'," +
                    $"'{periodicinspection[3]}'," +
                    $"'{periodicinspection[4]}'," +
                    $"'{periodicinspection[5]}'," +
                    $"'{periodicinspection[6]}'," +
                    $"'{periodicinspection[7]}')";
                if (ConnectionToDB.State != ConnectionState.Open)
                {
                    ConnectionToDB.Open();
                    SqlCommand PeriodicInspectionAdd = new SqlCommand(PeriodicInspectionAddCommand, ConnectionToDB)
                    {
                        CommandType = CommandType.Text
                    };
                    PeriodicInspectionAdd.ExecuteNonQuery();
                    ConnectionToDB.Close();
                    //CarMenu carMenu = new CarMenu();
                    //carMenu.Show();
                    MessageBox.Show("Data added successfuly");

                }
            else
            {
                MessageBox.Show("Error corupted Data");
            }
            }
            else
            {
                PeriodicInspectionUpdatetData();
                MessageBox.Show("Data updated");
            }
        }

        private void DataFromReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                var Ocr = new AdvancedOcr()
                {
                    CleanBackgroundNoise = true,
                    EnhanceContrast = true,
                    EnhanceResolution = false,
                    Language = IronOcr.Languages.Polish.OcrLanguagePack,
                    Strategy = IronOcr.AdvancedOcr.OcrStrategy.Fast,
                    ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
                    DetectWhiteTextOnDarkBackgrounds = false,
                    InputImageType = AdvancedOcr.InputTypes.Document,
                    RotateAndStraighten = true,
                    ReadBarCodes = false,
                    ColorDepth = 4
                };
                Bitmap img = new Bitmap(openFileDialog.FileName);
                var Results = Ocr.Read(img);
                DataFromReceiptTextBox.Text = Results.Text;
                foreach (var page in Results.Pages)
                {
                    // page object
                    foreach (var paragraph in page.Paragraphs)
                    {
                        // pages -> paragraphs
                        foreach (var line in paragraph.Lines)
                        {
                            // pages -> paragraphs -> lines
                            int line_number = line.LineNumber;
                            String line_text = line.Text;
                            foreach (var word in line.Words)
                            {
                                // pages -> paragraphs -> lines -> words
                                int word_number = word.WordNumber;
                                String word_text = word.Text;
                                if(word_number == 41)
                                {
                                    AddRefuelFuelTypeTextBox.Text = word_text;
                                }
                                if (word_number == 43)
                                {
                                    AddRefuelFuelAmountTextBox.Text = word_text;
                                }
                                if (word_number == 45)
                                {
                                    AddRefuelFuelPriceTextBox.Text = word_text;
                                }
                                if (word_number == 48)
                                {
                                    AddRefuelPriceTextBox.Text = word_text;
                                }
                                if (word_number == 85)
                                {
                                    AddRefuelDateDatePicker.Text = word_text;
                                }
                            }
                        }
                    }
                }
            }

        }
        private void AddRefuelDataResetButton_Click(object sender, RoutedEventArgs e)
        {
            AddRefuelLogDataClear();
        }

        private void AddRepairResetButton_Click(object sender, RoutedEventArgs e)
        {
            AddRepairLogDataClear();
        }

        private void PeriodicInspectionUpdateDataButton_Click(object sender, RoutedEventArgs e)
        {
            PeriodicInspectionSaveDataButton.IsEnabled = true;
            ActualRepairsStackPanel.IsEnabled = true;
            NextRepairsStackPanel.IsEnabled = true;
            LastFiltersChangeStackPanel.IsEnabled = true;
            NextFiltersChangeStackPanel.IsEnabled = true;
            ActualUsedOilTextBox.IsEnabled = true;
            if(PeriodicInspectionUpdateDataButton.IsInitialized == true && PeriodicInspectionUpdateDataButton.Content.ToString() == "Update")
            {
                PeriodicInspectionUpdateDataButton.Content = "Back";
            }else
            {
                PeriodicInspectionUpdateDataButton.Content = "Update";
                ActualRepairsStackPanel.IsEnabled = false;
                NextRepairsStackPanel.IsEnabled = false;
                LastFiltersChangeStackPanel.IsEnabled = false;
                NextFiltersChangeStackPanel.IsEnabled = false;
                ActualUsedOilTextBox.IsEnabled = false;
                PeriodicInspectionSaveDataButton.IsEnabled = false;
            }
        }
        #endregion
    }
}
