using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Windows;

namespace Weather_App
{
    class GeoLocator
    {
        private const string key = "e9e073292d6e4d488aed4410e73a807e";
        private static readonly string endpoint = $"https://api.ipgeolocation.io/ipgeo?apiKey={key}";

        public static MainWindow.Location LocateMe()
        {
            MainWindow.Location location = new MainWindow.Location();

            HttpClient client = new HttpClient();
            JObject json = null;

            try
            {
                string response = client.GetStringAsync(endpoint).Result;
                json = JObject.Parse(response);

                location.Latitude = Convert.ToString(json["latitude"]);
                location.Longitude = Convert.ToString(json["longitude"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error beim Ermitteln der Location", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return location;
        }
    }
}
