using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.SQLite;
using System.Windows.Markup;
using System.IO;
using System.Globalization;
using System.Windows;

namespace Weather_App
{
    class Cities
    {
        private static readonly string database = $"Data Source = {Directory.GetCurrentDirectory()}\\cities.db";
        private static SQLiteConnection connection = new SQLiteConnection(database);


        //Select cities from database where city name is like user entry
        public static List<string> SearchCity(string search)
        {
            List<string> foundCities = new List<string>();

            try
            {
                connection.Open();

                SQLiteDataReader reader;
                SQLiteCommand command;

                string statement = $"SELECT city, country FROM cities WHERE city Like '{search}%'";

                command = connection.CreateCommand();
                command.CommandText = statement;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string city = reader.GetString(0);
                    string country = reader.GetString(1);
                    string cityAndCountry = city + ", " + country;

                    foundCities.Add(cityAndCountry);
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error beim Lesen von der Datenbank", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return foundCities;
        }
        

        //Get latitude and longitude of the selected city from the database
        public static MainWindow.Location LocateCity(string city, string country)
        {
            MainWindow.Location location = new MainWindow.Location();

            try
            {
                connection.Open();

                SQLiteDataReader reader;
                SQLiteCommand command;

                string statement = $"SELECT lat, lng FROM cities WHERE city = '{city}' AND country = '{country}'";

                command = connection.CreateCommand();
                command.CommandText = statement;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    location.Latitude = reader.GetString(0);
                    location.Longitude = reader.GetString(1);
                }

                reader.Close();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error beim Lesen von der Datenbank", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return location;    
        }
    }
}
