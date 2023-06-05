using System.Data.SQLite;
using System.IO;
using System.Text;

namespace Weather_App
{
    class DBAccess
    {

        //Datenbank-Verbindungs-Objekt definiert, da es mehrfach innerhalb dieser Klasse benötigt wird.
        SQLiteConnection sqlite_conn;

        public DBAccess()
        {
            //--------------------------------------------------
            //      Hier die Verbindung zur DB herstellen.
            //--------------------------------------------------

            string connectionString = "Data Source=" + Directory.GetCurrentDirectory() + @"\DB\cities;"; 

            sqlite_conn = new SQLiteConnection(connectionString);
        }

        public string[] Check_City(string cityName, string countryName)
        {
            string city = null;
            string latitude = null;
            string longitude = null;
            string country = null;

            sqlite_conn.Open();

            SQLiteDataReader sqlite_DataReader;
            SQLiteCommand sqlite_Command;

            sqlite_Command = sqlite_conn.CreateCommand();

            string statement = $"SELECT lat, lng FROM cities WHERE city = '{cityName}' AND country = '{countryName}';";

            sqlite_Command.CommandText = statement;

            sqlite_DataReader = sqlite_Command.ExecuteReader();

            while (sqlite_DataReader.Read())
            {

                latitude = sqlite_DataReader.GetString(0);
                longitude = sqlite_DataReader.GetString(1);
            }

            sqlite_DataReader.Close();
            sqlite_conn.Close();

            string[] coordinates = {latitude, longitude};

            return coordinates;
        }
    }
}
