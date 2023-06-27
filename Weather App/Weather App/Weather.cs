using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Windows;

namespace Weather_App
{
    class Weather
    {
        private const string key = "585268ff92324b56990155655230605";

        public static JObject GetWeatherInformation(string latitude, string longitude)
        {
            JObject json = new JObject();

            string endpoint = $"http://api.weatherapi.com/v1/current.json?Key={key}&q={latitude},{longitude}";

            HttpClient client = new HttpClient();

            try
            {
                string response = client.GetStringAsync(endpoint).Result;
                json = JObject.Parse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error beim Ermitteln der Location", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return json;
        }
    }
}
