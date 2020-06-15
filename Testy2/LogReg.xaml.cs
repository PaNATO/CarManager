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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Testy2
{
    /// <summary>
    /// Logika interakcji dla klasy LogReg.xaml
    /// </summary>
    public partial class LogReg : Window
    {
        readonly string connectionString = Properties.Settings.Default.ConnectionToDB;
        public LogReg()
        {
            InitializeComponent();
        }
        private void LoginRegDataClear()
        {
            UNameLogInput.Text = UNamePassLogInput.Password = UNameRegInput.Text = UPassRegInput.Password = UPassAgRegInput.Password = null;
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool poprawnoscDanych = false;
            string UserID;
            string LoginString = UNameLogInput.Text;
            string PasswordString = UNamePassLogInput.Password.ToString();
            SqlConnection ConnectionDB = new SqlConnection(connectionString);
            string LoginCommand = "Select UserID, Username, Password from Users where Username= '" + LoginString + "' and Password='" + PasswordString + "'";
            if (ConnectionDB.State != ConnectionState.Open)
            {
                ConnectionDB.Open();
                SqlCommand LoginToDB = new SqlCommand(LoginCommand, ConnectionDB)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader LoginReader;
                LoginReader = LoginToDB.ExecuteReader();
                while (LoginReader.Read()) 
                {
                    if (!LoginReader.IsDBNull(0))
                    {
                        poprawnoscDanych = true;
                        UserID = LoginReader["UserID"].ToString();
                        CarMenu CarData = new CarMenu
                        {
                            UName = LoginString,
                            UserID = UserID
                        };
                        CarData.Wyświetl();
                        CarData.UserCarsListFunc();
                        MessageBox.Show("User Loged in");
                        this.Close();
                        CarData.Show();
                        LoginRegDataClear();
                    }
                }
                if (!poprawnoscDanych)
                {
                    MessageBox.Show("User do not exist");
                }
                ConnectionDB.Close();
                //if()
                //{
                
                // }

            }
            else
            {
                MessageBox.Show("Connection failed");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            bool istniejaceDane = false;
            string UsernameStrig = UNameRegInput.Text;
            string UserPassString = UPassRegInput.Password.ToString();
            string UserPassAgString = UPassAgRegInput.Password.ToString();
            SqlConnection ConnectionDB = new SqlConnection(connectionString);
            string RegisterCheck = "Select Username from Users where Username= '" + UsernameStrig + "'";
            if (ConnectionDB.State != ConnectionState.Open)
            {
                ConnectionDB.Open();
                SqlCommand CheckRegisteredDB = new SqlCommand(RegisterCheck, ConnectionDB)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader RegisterReader;
                RegisterReader = CheckRegisteredDB.ExecuteReader();
                    if ((RegisterReader.Read()) == true)
                    {
                        istniejaceDane = true;
                    }

                ConnectionDB.Close();
                if (!istniejaceDane)
                {
                    if (UserPassString == UserPassAgString)
                    {
                        ConnectionDB.Open();
                        string RegisterCommand = $"Insert into Users(Username,Password)values(' { UsernameStrig } ','{ UserPassString }')";
                        SqlCommand RegisterNewDB = new SqlCommand(RegisterCommand, ConnectionDB)
                        {
                            CommandType = CommandType.Text
                        };
                        RegisterNewDB.ExecuteNonQuery();
                        ConnectionDB.Close();
                        LoginRegDataClear();
                        MessageBox.Show("Register successful!");
                    }
                    else
                    {
                        MessageBox.Show("Passwords are not equal!");
                    }
                }else
                {
                    MessageBox.Show("User already exist!");
                }
            }
            else
            {
                MessageBox.Show("Connection failed!");
            }
        }
    }
}
