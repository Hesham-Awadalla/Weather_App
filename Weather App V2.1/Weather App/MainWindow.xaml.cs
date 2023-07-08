using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Weather_App
{
    /// <summary>
    /// Backend-Developers:     Maurice Hofmann
    ///                         Hesham Mohamed Awadalla Osman
    /// </summary>
    public partial class MainWindow : Window
    {
        public struct Location
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }

        Location locationMain = new Location();
        Location locationDefault1 = new Location();
        Location locationDefault2 = new Location();
        Location locationDefault3 = new Location();

        public static int stdMainLocalTime;
        public static int stdLocation1LocalTime;
        public static int stdLocation2LocalTime;
        public static int stdLocation3LocalTime;

        System.Windows.Input.MouseButtonEventArgs e;

        public MainWindow()
        {
            InitializeComponent();

            imgLocateMe_Click(null, e);
            GetDefaultLocation();
            GetWeatherData(null, new EventArgs());
            InitializeRefreshWeatherTimer();

            Clock mainLocationClock = new Clock(30, "canvasMainClock");
            Clock location1Clock = new Clock(15, "canvasClock1");
            Clock location2Clock = new Clock(15, "canvasClock2");
            Clock location3Clock = new Clock(15, "canvasClock3");
        }

        public void InitializeRefreshWeatherTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(GetWeatherData);
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Start();
        }

        private void imgLocateMe_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            locationMain = GeoLocator.LocateMe();
            JObject weatherDataMain = Weather.GetWeatherInformation(locationMain.Latitude, locationMain.Longitude);

            //Set Main-Location values
            lblMainTemp.Content = weatherDataMain["current"]["temp_c"] + "°C";
            lblMainCity.Content = weatherDataMain["location"]["name"];
            lblMainCountry.Content = weatherDataMain["location"]["country"];
            imgMain.Source = new BitmapImage(new Uri("https:" + weatherDataMain["current"]["condition"]["icon"].ToString()));
            stdMainLocalTime = Convert.ToInt32(weatherDataMain["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);    //StundenZeiger

        }

        public void GetDefaultLocation()
        {
            JObject defaultLocation = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\LocationStorage.json"));

            locationDefault1.Latitude = Convert.ToString(defaultLocation["location1"]["latitude"]);
            locationDefault1.Longitude = Convert.ToString(defaultLocation["location1"]["longitude"]);

            locationDefault2.Latitude = Convert.ToString(defaultLocation["location2"]["latitude"]);
            locationDefault2.Longitude = Convert.ToString(defaultLocation["location2"]["longitude"]);

            locationDefault3.Latitude = Convert.ToString(defaultLocation["location3"]["latitude"]);
            locationDefault3.Longitude = Convert.ToString(defaultLocation["location3"]["longitude"]);
        }

        public void GetWeatherData(object sender, EventArgs e)
        {
            JObject weatherDataMain = Weather.GetWeatherInformation(locationMain.Latitude, locationMain.Longitude);
            JObject weatherDataLocation1 = Weather.GetWeatherInformation(locationDefault1.Latitude, locationDefault1.Longitude);
            JObject weatherDataLocation2 = Weather.GetWeatherInformation(locationDefault2.Latitude, locationDefault2.Longitude);
            JObject weatherDataLocation3 = Weather.GetWeatherInformation(locationDefault3.Latitude, locationDefault3.Longitude);


            //Set Main-Location values
            lblMainTemp.Content = weatherDataMain["current"]["temp_c"] + "°C";
            lblMainCity.Content = weatherDataMain["location"]["name"];
            lblMainCountry.Content = weatherDataMain["location"]["country"];
            imgMain.Source = new BitmapImage(new Uri("https:" + weatherDataMain["current"]["condition"]["icon"].ToString()));

            //Set additional weather data
            lblWind.Content = weatherDataMain["current"]["wind_kph"] + " km/h";
            lblPressure.Content = weatherDataMain["current"]["pressure_mb"] + " mb";
            lblHumidity.Content = weatherDataMain["current"]["humidity"] + "%";
            stdMainLocalTime = Convert.ToInt32(weatherDataMain["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);


            //Set Location 1 values
            lblLocation1Temp.Content = weatherDataLocation1["current"]["temp_c"] + "°C";
            lblLocation1City.Content = weatherDataLocation1["location"]["name"];
            lblLocation1Country.Content = weatherDataLocation1["location"]["country"];
            imgLocation1.Source = new BitmapImage(new Uri("https:" + weatherDataLocation1["current"]["condition"]["icon"].ToString()));
            stdLocation1LocalTime = Convert.ToInt32(weatherDataLocation1["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);


            //Set Location 2 values
            lblLocation2Temp.Content = weatherDataLocation2["current"]["temp_c"] + "°C";
            lblLocation2City.Content = weatherDataLocation2["location"]["name"];
            lblLocation2Country.Content = weatherDataLocation2["location"]["country"];
            imgLocation2.Source = new BitmapImage(new Uri("https:" + weatherDataLocation2["current"]["condition"]["icon"].ToString()));
            stdLocation2LocalTime = Convert.ToInt32(weatherDataLocation2["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);


            //Set Location 3 values
            lblLocation3Temp.Content = weatherDataLocation3["current"]["temp_c"] + "°C";
            lblLocation3City.Content = weatherDataLocation3["location"]["name"];
            lblLocation3Country.Content = weatherDataLocation3["location"]["country"];
            imgLocation3.Source = new BitmapImage(new Uri("https:" + weatherDataLocation3["current"]["condition"]["icon"].ToString()));
            stdLocation3LocalTime = Convert.ToInt32(weatherDataLocation3["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);

            //Update
            lblLastUpdated.Content = "Last updated: " + weatherDataMain["current"]["last_updated"];
            this.UpdateLayout();
        }

        private void txtSearchCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<string> foundCities = Cities.SearchCity(txtSearchCity.Text);
            lstFoundCitys.ItemsSource = foundCities;

            if (foundCities.Count > 0 && txtSearchCity.Text.Length > 0)
            {
                lstFoundCitys.Visibility = Visibility.Visible;
            }
            else
            {
                lstFoundCitys.Visibility = Visibility.Collapsed;
            }
        }

        private void lstFoundCitys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFoundCitys.SelectedItem != null)
            {
                string selectedCity = lstFoundCitys.SelectedItem.ToString();

                string[] cityCountry = selectedCity.Split(", ");
                string city = cityCountry[0];
                string country = cityCountry[1];

                locationMain = Cities.LocateCity(city, country);
                JObject weatherDataMain = Weather.GetWeatherInformation(locationMain.Latitude, locationMain.Longitude);

                lblMainTemp.Content = weatherDataMain["current"]["temp_c"] + "°C";
                lblMainCity.Content = weatherDataMain["location"]["name"];
                lblMainCountry.Content = weatherDataMain["location"]["country"];
                imgMain.Source = new BitmapImage(new Uri("https:" + weatherDataMain["current"]["condition"]["icon"].ToString()));

                lblWind.Content = weatherDataMain["current"]["wind_kph"] + " km/h";
                lblPressure.Content = weatherDataMain["current"]["pressure_mb"] + " mb";
                lblHumidity.Content = weatherDataMain["current"]["humidity"] + "%";

                stdMainLocalTime = Convert.ToInt32(weatherDataMain["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);
            }

            txtSearchCity.Text = "";
            lstFoundCitys.SelectedItem = null;
        }

        private void imgLocation1Change_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            JObject locationStorage = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\LocationStorage.json"));

            locationStorage["location1"]["latitude"] = locationMain.Latitude;
            locationStorage["location1"]["longitude"] = locationMain.Longitude;

            File.WriteAllText("LocationStorage.json", Convert.ToString(locationStorage));

            locationDefault1.Latitude = Convert.ToString(locationStorage["location1"]["latitude"]);
            locationDefault1.Longitude = Convert.ToString(locationStorage["location1"]["longitude"]);

            JObject weatherDataLocation1 = Weather.GetWeatherInformation(locationDefault1.Latitude, locationDefault1.Longitude);

            lblLocation1Temp.Content = weatherDataLocation1["current"]["temp_c"] + "°C";
            lblLocation1City.Content = weatherDataLocation1["location"]["name"];
            lblLocation1Country.Content = weatherDataLocation1["location"]["country"];
            imgLocation1.Source = new BitmapImage(new Uri("https:" + weatherDataLocation1["current"]["condition"]["icon"].ToString()));
            stdLocation1LocalTime = Convert.ToInt32(weatherDataLocation1["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);
        }

        private void imgLocation2Change_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            JObject locationStorage = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\LocationStorage.json"));

            locationStorage["location2"]["latitude"] = locationMain.Latitude;
            locationStorage["location2"]["longitude"] = locationMain.Longitude;

            File.WriteAllText("LocationStorage.json", Convert.ToString(locationStorage));

            locationDefault2.Latitude = Convert.ToString(locationStorage["location2"]["latitude"]);
            locationDefault2.Longitude = Convert.ToString(locationStorage["location2"]["longitude"]);

            JObject weatherDataLocation2 = Weather.GetWeatherInformation(locationDefault2.Latitude, locationDefault2.Longitude);

            lblLocation2Temp.Content = weatherDataLocation2["current"]["temp_c"] + "°C";
            lblLocation2City.Content = weatherDataLocation2["location"]["name"];
            lblLocation2Country.Content = weatherDataLocation2["location"]["country"];
            imgLocation2.Source = new BitmapImage(new Uri("https:" + weatherDataLocation2["current"]["condition"]["icon"].ToString()));

            stdLocation2LocalTime = Convert.ToInt32(weatherDataLocation2["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);
        }

        private void imgLocation3Change_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            JObject locationStorage = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\LocationStorage.json"));

            locationStorage["location3"]["latitude"] = locationMain.Latitude;
            locationStorage["location3"]["longitude"] = locationMain.Longitude;

            File.WriteAllText("LocationStorage.json", Convert.ToString(locationStorage));

            locationDefault3.Latitude = Convert.ToString(locationStorage["location3"]["latitude"]);
            locationDefault3.Longitude = Convert.ToString(locationStorage["location3"]["longitude"]);

            JObject weatherDataLocation3 = Weather.GetWeatherInformation(locationDefault3.Latitude, locationDefault3.Longitude);

            lblLocation3Temp.Content = weatherDataLocation3["current"]["temp_c"] + "°C";
            lblLocation3City.Content = weatherDataLocation3["location"]["name"];
            lblLocation3Country.Content = weatherDataLocation3["location"]["country"];
            imgLocation3.Source = new BitmapImage(new Uri("https:" + weatherDataLocation3["current"]["condition"]["icon"].ToString()));

            stdLocation3LocalTime = Convert.ToInt32(weatherDataLocation3["location"]["localtime"].ToString().Split(" ")[1].Split(':')[0]);
        }
    }
}