using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Weather_App
{
    class APICall
    {
        static DBAccess DB_Access = new DBAccess();

        static string latitude, longitude;
        static readonly string WetterURL = "http://api.weatherapi.com/v1/current.json";
        static readonly string WetterKey = "585268ff92324b56990155655230605";
        static string WetterUrlParam = $"?Key={WetterKey}&q={latitude},{longitude}";

        static readonly string LocationURL = "https://api.ipgeolocation.io/ipgeo";
        static readonly string LocationKey = "e9e073292d6e4d488aed4410e73a807e";
        static string LocationUrlParam = $"?apiKey={LocationKey}&lang=en"; //(&lang=??) Response in Multiple Languages

        public static void LocateMe()
        {
            JObject jsonL = API_Call(LocationURL, LocationUrlParam);

            latitude = jsonL["latitude"].ToString();
            longitude = jsonL["longitude"].ToString();
            WetterUrlParam = $"?Key={WetterKey}&q={latitude},{longitude}";

            JObject jsonW = API_Call(WetterURL, WetterUrlParam);

            string ausgabe = "Location:\n" + jsonW["location"]["name"] + ", " + jsonW["location"]["country"]
                           + "\n\nLocal time: " + jsonW["location"]["localtime"]
                           + "\nTemperatur: " + jsonW["current"]["temp_c"] + " °C | " + jsonW["current"]["temp_f"] + " °F"
                           + "\nWind: " + jsonW["current"]["wind_kph"] + " km/h"
                           + "\nPressure: " + jsonW["current"]["pressure_mb"] + " mb"
                           + "\nHumidity: " + jsonW["current"]["humidity"] + "%";

            ((Label)App.Current.Resources["lblWetterDaten"]).Content = ausgabe;
            ((Image)App.Current.Resources["img"]).Source = new BitmapImage(new Uri("http:"+jsonW["current"]["condition"]["icon"].ToString()));

        }

        public static void SearchMyCity(string cityName, string countryName)
        {
            string[] coordinates = DB_Access.Check_City(cityName, countryName);

            latitude = coordinates[0];
            longitude = coordinates[1];

            string WetterUrlParam = $"?Key={WetterKey}&q={latitude},{longitude}";

            JObject jsonW = API_Call(WetterURL, WetterUrlParam);

            //ToDo: try & catch (location)
            string ausgabe = "Location:\n" + jsonW["location"]["name"] + ", " + jsonW["location"]["country"]
                           + "\n\nLocal time: " + jsonW["location"]["localtime"]
                           + "\nTemperatur: " + jsonW["current"]["temp_c"] + " °C | " + jsonW["current"]["temp_f"] + " °F"
                           + "\nWind: " + jsonW["current"]["wind_kph"] + " km/h"
                           + "\nPressure: " + jsonW["current"]["pressure_mb"] + " mb"
                           + "\nHumidity: " + jsonW["current"]["humidity"] + "%";

            ((Label)App.Current.Resources["lblWetterDaten"]).Content = ausgabe;
            ((Image)App.Current.Resources["img"]).Source = new BitmapImage(new Uri("http:" + jsonW["current"]["condition"]["icon"].ToString()));
        }

        private static JObject API_Call(string url, string urlParam)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            JObject json = null;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParam).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.

            if (response.IsSuccessStatusCode)
            {
                json = response.Content.ReadAsAsync<JObject>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            // Make any other calls using HttpClient here.

            // Dispose once all HttpClient calls are complete.
            // This is not necessary if the containing object will be disposed of;
            // for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();

            return json;
        }
    }
}
