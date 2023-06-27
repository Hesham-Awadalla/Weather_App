using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        public MainWindow()
        {
            InitializeComponent();

            btnLocateMe.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            GetDefaultLocation();
            GetWeatherData(null, new EventArgs());

            InitializeRefreshWeatherTimer();
        }

        public void InitializeRefreshWeatherTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(GetWeatherData);
            timer.Interval = new TimeSpan(0, 3, 0);
            timer.Start();
        }

        private void btnLocateMe_Click(object sender, RoutedEventArgs e)
        {
            locationMain = GeoLocator.LocateMe();
            JObject weatherDataMain = Weather.GetWeatherInformation(locationMain.Latitude, locationMain.Longitude);

            lblMainTemp.Content = weatherDataMain["current"]["temp_c"] + "°C";
            lblMainCity.Content = weatherDataMain["location"]["name"];
            lblMainCountry.Content = weatherDataMain["location"]["country"];
            imgMain.Source = new BitmapImage(new Uri("https:" + weatherDataMain["current"]["condition"]["icon"].ToString()));
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

            lblMainTemp.Content = weatherDataMain["current"]["temp_c"] + "°C";
            lblMainCity.Content = weatherDataMain["location"]["name"];
            lblMainCountry.Content = weatherDataMain["location"]["country"];
            imgMain.Source = new BitmapImage(new Uri("https:" + weatherDataMain["current"]["condition"]["icon"].ToString()));

            string additionalWeatherData = "Wind: " + weatherDataMain["current"]["wind_kph"] + " km/h"
            + "\n\nPressure: " + weatherDataMain["current"]["pressure_mb"] + " mb"
            + "\n\nHumidity: " + weatherDataMain["current"]["humidity"] + "%"
            + "\n\nLocal time: " + weatherDataMain["location"]["localtime"];

            lblAdditionalWeatherData.Content = additionalWeatherData;

            lblLocation1Temp.Content = weatherDataLocation1["current"]["temp_c"] + "°C";
            lblLocation1City.Content = weatherDataLocation1["location"]["name"];
            imgLocation1.Source = new BitmapImage(new Uri("https:" + weatherDataLocation1["current"]["condition"]["icon"].ToString()));

            lblLocation2Temp.Content = weatherDataLocation2["current"]["temp_c"] + "°C";
            lblLocation2City.Content = weatherDataLocation2["location"]["name"];
            imgLocation2.Source = new BitmapImage(new Uri("https:" + weatherDataLocation2["current"]["condition"]["icon"].ToString()));

            lblLocation3Temp.Content = weatherDataLocation3["current"]["temp_c"] + "°C";
            lblLocation3City.Content = weatherDataLocation3["location"]["name"];
            imgLocation3.Source = new BitmapImage(new Uri("https:" + weatherDataLocation3["current"]["condition"]["icon"].ToString()));

            lblLastUpdated.Content = "Last updated: " + weatherDataMain["current"]["last_updated"];
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

                string additionalWeatherData = "Wind: " + weatherDataMain["current"]["wind_kph"] + " km/h"
                + "\n\nPressure: " + weatherDataMain["current"]["pressure_mb"] + " mb"
                + "\n\nHumidity: " + weatherDataMain["current"]["humidity"] + "%"
                + "\n\nLocal time: " + weatherDataMain["location"]["localtime"];

                lblAdditionalWeatherData.Content = additionalWeatherData;
            }

            txtSearchCity.Text = "";
            lstFoundCitys.SelectedItem = null;
        }

        //Now: Change to MainLocation --> Independent Search
        private async void btnLocation1Change_Click(object sender, RoutedEventArgs e)
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
            imgLocation1.Source = new BitmapImage(new Uri("https:" + weatherDataLocation1["current"]["condition"]["icon"].ToString()));
        }

        private void btnLocation2Change_Click(object sender, RoutedEventArgs e)
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
            imgLocation2.Source = new BitmapImage(new Uri("https:" + weatherDataLocation2["current"]["condition"]["icon"].ToString()));
        }

        private void btnLocation3Change_Click(object sender, RoutedEventArgs e)
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
            imgLocation3.Source = new BitmapImage(new Uri("https:" + weatherDataLocation3["current"]["condition"]["icon"].ToString()));
        }

    }
}
