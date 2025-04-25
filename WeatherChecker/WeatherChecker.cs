using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Vishnu.Interchange;
using Windows_Geolocation;

namespace WeatherChecker
{
    /// <summary>
    /// DemoChecker-Projekt für Vishnu.
    /// Die WeatherChecker.dll kann später in einer Vishnu-JobDescription.xml referenziert
    /// werden und von Vishnu bei der Ausführung des Jobs aus dem Plugin-Unterverzeichnis
	/// oder einem konfigurierten UserAssemblyDirectory dynamisch geladen werden.
    /// Vishnu ruft dann je nach weiteren Konfigurationen (Trigger) die öffentliche
    /// Methode "Run" des UserCheckers auf.
    /// </summary>
    /// <remarks>
    /// Autor: Erik Nagel
    ///
    /// The weather forecasts come from https://open-meteo.com. Thanks to the team.
    /// Further I want to thank the developer-teams of https://get.geojs.io/v1/ip/geo.json
    /// and http://ip-api.com/ who provide geolocation services, which are free for non commercial usage.
    /// 
    /// (The weather forecasts formerly came from https://www.7timer.info.
    ///  Many thanks to Chenzhou Cui and his friends, who runned this wonderful free weather-forecast site)
    /// 
    /// 11.10.2020 Erik Nagel: erstellt
    /// 26.04.2024 Erik Nagel: auf open.meteo.com geaendert.
    /// 14.09.2024 Erik Nagel: für die zusätzliche Darstellung von Einzelinformationen erweitert.
    /// 10.04.2025 Erik Nagel: für höhere Genauigkeit die Windows-Standort Abfrage hinzugefügt.
    /// </remarks>
    public class WeatherChecker : INodeChecker, IDisposable
    {
        #region INodeChecker Implementation

        /// <summary>
        /// Kann aufgerufen werden, wenn sich der Verarbeitungsfortschritt
        /// des Checkers geändert hat, muss aber zumindest aber einmal zum
        /// Schluss der Verarbeitung aufgerufen werden.
        /// </summary>
        public event ProgressChangedEventHandler? NodeProgressChanged;

        /// <summary>
        /// Rückgabe-Objekt des Checkers
        /// </summary>
        public object? ReturnObject
        {
            get
            {
                return this._returnObject;
            }
            set
            {
                this._returnObject = value;
            }
        }

        /// <summary>
        /// Hier wird der Arbeitsprozess ausgeführt (oder beobachtet).
        /// </summary>
        /// <param name="checkerParameters">Ihre Aufrufparameter aus der JobDescription.xml oder null.</param>
        /// <param name="treeParameters">Für den gesamten Tree gültige Parameter oder null (z.Zt. unbenutzt).</param>
        /// <param name="source">Auslösendes TreeEvent (kann null sein).</param>
        /// <returns>True, False oder null</returns>
        public bool? Run(object? checkerParameters, TreeParameters treeParameters, TreeEvent source)
        {
            this.OnNodeProgressChanged(0);
            this._returnObject = null; // optional: in WeatherChecker_ReturnObject IDisposable implementieren und hier aufrufen.
            Thread.Sleep(200); // nur zu Demonstrationszwecken, damit man in Vishnu einen Verarbeitungsfortschritt sieht.
            this.OnNodeProgressChanged(50); // nur zu Demonstrationszwecken, kann wegfallen.
            //--- Aufruf der Checker-Business-Logik ----------
            bool? returnCode = Task.Run(() => this.Work(source)).Result;
            // bool? returnCode = this.Work(source).Wait();
            //------------------------------------------------
            this.OnNodeProgressChanged(100); // erforderlich!
            return returnCode; // 
        }

        #endregion INodeChecker Implementation

        #region IDisposable Implementation

        private bool _disposed = false;

        /// <summary>
        /// Öffentliche Methode zum Aufräumen.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Abschlussarbeiten.
        /// </summary>
        /// <param name="disposing">False, wenn vom eigenen Destruktor aufgerufen.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // Aufräumarbeiten durchführen und dann beenden.
                }
                this._disposed = true;
            }
        }

        /// <summary>
        /// Finalizer: wird vom GarbageCollector aufgerufen.
        /// </summary>
        ~WeatherChecker()
        {
            this.Dispose(false);
        }

        #endregion IDisposable Implementation

        private object? _returnObject = null;

        private async Task<GeoLocation_ReturnObject?> GetWindowsLocation()
        {
            LocationService locationService = new LocationService();

            Windows.Devices.Geolocation.Geoposition result = await locationService.GetCurrentLocationAsync();
            GeoLocation_ReturnObject? returnObject = new GeoLocation_ReturnObject()
            {
                Latitude = result.Coordinate.Point.Position.Latitude,
                Longitude = result.Coordinate.Point.Position.Longitude,
                Elevation = result.Coordinate.Point.Position.Altitude
            };

            return returnObject;
        }

        private async Task<GeoLocation_ReturnObject?> GetGeoLocation()
        {
            // Uri uri = new Uri(@"http://ip-api.com/json");
            Uri uri = new Uri(@"https://get.geojs.io/v1/ip/geo.json");
            /*
            {
                "country_code": "DE",
                "country_code3": "DEU",
                "continent_code": "EU",
                "region": "North Rhine-Westphalia",
                "latitude": "51.2402",
                "longitude": "6.7785",
                "accuracy": 5,
                "country": "Germany",
                "timezone": "Europe/Berlin",
                "city": "Düsseldorf",
                "organization": "AS3320 Deutsche Telekom AG",
                "asn": 3320,
                "ip": "87.146.129.31",
                "area_code": "0",
                "organization_name": "Deutsche Telekom AG"
            }            
            */
            // Geolocation Standort Position Api 

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10);
            HttpResponseMessage? response = null;
            string? responseJsonString = null;
            GeoLocation_geojs_ReturnObject? deserializedLocation = null;
            try
            {
                response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                responseJsonString = await response.Content.ReadAsStringAsync();
                deserializedLocation = JsonConvert.DeserializeObject<GeoLocation_geojs_ReturnObject>(responseJsonString);
            }
            catch (HttpRequestException)
            {
                // Handle exception here
                throw;
                // return null;
            }
            finally
            {
                response?.Dispose();
                client?.Dispose();
            }

            return GeoLocation_geojs_ReturnObject.ConvertTo_GeoLocation_ReturnObject(deserializedLocation);
        }

        private async Task<bool?> Work(TreeEvent source)
        {
            string? longitude = "6.7735";  // fallback
            string? latitude = "51.2277";  // = Düsseldorf

            // Latitude = 53.545803344748855, Longitude = 9.9729329908675783 Hamburg

            GeoLocation_ReturnObject? geoLocation_ReturnObject = Task.Run(() => this.GetWindowsLocation()).Result;
            if (geoLocation_ReturnObject != null)
            {
                longitude = geoLocation_ReturnObject.Longitude.ToString()?.Replace(',', '.');
                latitude = geoLocation_ReturnObject.Latitude.ToString()?.Replace(',', '.');
            }
            else
            {
                geoLocation_ReturnObject = Task.Run(() => this.GetGeoLocation()).Result;
            }

            if (geoLocation_ReturnObject != null)
            {
                longitude = geoLocation_ReturnObject.Longitude.ToString()?.Replace(',', '.');
                latitude = geoLocation_ReturnObject.Latitude.ToString()?.Replace(',', '.');
            }

            // Hier folgt die eigentliche Checker-Verarbeitung, die einen erweiterten boolean als Rückgabe
            // dieses Checkers ermittelt und das WeatherChecker_ReturnObject mit zusätzlichen Informationen
            // aus dem nachfolgenden API-Aufruf der Seite www.7timer.info füllt.

            // Uri uri = new Uri(@"https://www.7timer.info/bin/api.pl?lon=" + longitude + "&lat=" + latitude + "&product=civil&output=json");
            Uri uri = new Uri("https://api.open-meteo.com/v1/forecast"
                          + "?latitude=" + latitude
                          + "&longitude=" + longitude
                          + "&timezone=Europe%2FBerlin"
                          + "&forecast_days=7"
                          + "&hourly=temperature_2m"
                          + ",relative_humidity_2m"
                          + ",apparent_temperature"
                          + ",precipitation_probability"
                          + ",rain"
                          + ",showers"
                          + ",snowfall"
                          + ",weather_code"
                          + ",cloud_cover"
                          + ",surface_pressure"
                          + ",wind_speed_10m"
                          + ",wind_direction_10m"
                          + ",wind_gusts_10m"
                          + "&daily=temperature_2m_max"
                          + ",temperature_2m_min"
                          + ",apparent_temperature_min"
                          + ",apparent_temperature_max"
                          + ",sunrise"
                          + ",sunset"
                          + ",sunshine_duration"
                          + ",rain_sum"
                          + ",showers_sum"
                          + ",snowfall_sum");

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10);
            HttpResponseMessage? response = null;
            string? responseJsonString = null;
            WeatherChecker_OpenMeteo_ReturnObject? deserializedWeather = null;
            try
            {
                response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                responseJsonString = await response.Content.ReadAsStringAsync();
                deserializedWeather = JsonConvert.DeserializeObject<WeatherChecker_OpenMeteo_ReturnObject>(responseJsonString);
            }
            catch (HttpRequestException)
            {
                // Handle exception here
                throw;
                // return null;
            }
            finally
            {
                response?.Dispose();
                client?.Dispose();
            }
            this._returnObject = WeatherChecker_OpenMeteo_ReturnObject.ConvertToWeatherChecker_ReturnObject(deserializedWeather, geoLocation_ReturnObject);
            return true;

            /*
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            HttpWebResponse response;
			WeatherChecker_ReturnObject deserializedWeather = null;
            string responseJsonString = null;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseJsonString = reader.ReadToEnd();
                }
                deserializedWeather = JsonConvert.DeserializeObject<WeatherChecker_ReturnObject>(responseJsonString);
            }
            catch (Exception ex) when (ex is WebException || ex is ArgumentException || ex is ArgumentNullException)
            {
                return null;
            }
            */
        }

        private void OnNodeProgressChanged(int progressPercentage)
        {
            NodeProgressChanged?.Invoke(null, new ProgressChangedEventArgs(progressPercentage, null));
        }

    }
}

/*
https://open-meteo.com/en/docs/dwd-api#hourly=temperature_2m,relative_humidity_2m,weather_code
{
    "latitude": 51.22,
    "longitude": 6.7799993,
    "generationtime_ms": 0.06103515625,
    "utc_offset_seconds": 7200,
    "timezone": "Europe/Berlin",
    "timezone_abbreviation": "CEST",
    "elevation": 45.0,
    "hourly_units": {
        "time": "iso8601",
        "temperature_2m": "°C",
        "relative_humidity_2m": "%",
        "weather_code": "wmo code"
    },
    "hourly": {
        "time": [
            "2024-04-25T00:00",
            "2024-04-25T01:00",
            "2024-04-25T02:00",
            "2024-04-25T03:00",
            "2024-04-25T04:00",
            "2024-04-25T05:00",
            "2024-04-25T06:00",
            "2024-04-25T07:00",
            "2024-04-25T08:00",
            "2024-04-25T09:00",
            "2024-04-25T10:00",
            "2024-04-25T11:00",
            "2024-04-25T12:00",
            "2024-04-25T13:00",
            "2024-04-25T14:00",
            "2024-04-25T15:00",
            "2024-04-25T16:00",
            "2024-04-25T17:00",
            "2024-04-25T18:00",
            "2024-04-25T19:00",
            "2024-04-25T20:00",
            "2024-04-25T21:00",
            "2024-04-25T22:00",
            "2024-04-25T23:00",
            "2024-04-26T00:00",
            "2024-04-26T01:00",
            "2024-04-26T02:00",
            "2024-04-26T03:00",
            "2024-04-26T04:00",
            "2024-04-26T05:00",
            "2024-04-26T06:00",
            "2024-04-26T07:00",
            "2024-04-26T08:00",
            "2024-04-26T09:00",
            "2024-04-26T10:00",
            "2024-04-26T11:00",
            "2024-04-26T12:00",
            "2024-04-26T13:00",
            "2024-04-26T14:00",
            "2024-04-26T15:00",
            "2024-04-26T16:00",
            "2024-04-26T17:00",
            "2024-04-26T18:00",
            "2024-04-26T19:00",
            "2024-04-26T20:00",
            "2024-04-26T21:00",
            "2024-04-26T22:00",
            "2024-04-26T23:00",
            "2024-04-27T00:00",
            "2024-04-27T01:00",
            "2024-04-27T02:00",
            "2024-04-27T03:00",
            "2024-04-27T04:00",
            "2024-04-27T05:00",
            "2024-04-27T06:00",
            "2024-04-27T07:00",
            "2024-04-27T08:00",
            "2024-04-27T09:00",
            "2024-04-27T10:00",
            "2024-04-27T11:00",
            "2024-04-27T12:00",
            "2024-04-27T13:00",
            "2024-04-27T14:00",
            "2024-04-27T15:00",
            "2024-04-27T16:00",
            "2024-04-27T17:00",
            "2024-04-27T18:00",
            "2024-04-27T19:00",
            "2024-04-27T20:00",
            "2024-04-27T21:00",
            "2024-04-27T22:00",
            "2024-04-27T23:00",
            "2024-04-28T00:00",
            "2024-04-28T01:00",
            "2024-04-28T02:00",
            "2024-04-28T03:00",
            "2024-04-28T04:00",
            "2024-04-28T05:00",
            "2024-04-28T06:00",
            "2024-04-28T07:00",
            "2024-04-28T08:00",
            "2024-04-28T09:00",
            "2024-04-28T10:00",
            "2024-04-28T11:00",
            "2024-04-28T12:00",
            "2024-04-28T13:00",
            "2024-04-28T14:00",
            "2024-04-28T15:00",
            "2024-04-28T16:00",
            "2024-04-28T17:00",
            "2024-04-28T18:00",
            "2024-04-28T19:00",
            "2024-04-28T20:00",
            "2024-04-28T21:00",
            "2024-04-28T22:00",
            "2024-04-28T23:00",
            "2024-04-29T00:00",
            "2024-04-29T01:00",
            "2024-04-29T02:00",
            "2024-04-29T03:00",
            "2024-04-29T04:00",
            "2024-04-29T05:00",
            "2024-04-29T06:00",
            "2024-04-29T07:00",
            "2024-04-29T08:00",
            "2024-04-29T09:00",
            "2024-04-29T10:00",
            "2024-04-29T11:00",
            "2024-04-29T12:00",
            "2024-04-29T13:00",
            "2024-04-29T14:00",
            "2024-04-29T15:00",
            "2024-04-29T16:00",
            "2024-04-29T17:00",
            "2024-04-29T18:00",
            "2024-04-29T19:00",
            "2024-04-29T20:00",
            "2024-04-29T21:00",
            "2024-04-29T22:00",
            "2024-04-29T23:00",
            "2024-04-30T00:00",
            "2024-04-30T01:00",
            "2024-04-30T02:00",
            "2024-04-30T03:00",
            "2024-04-30T04:00",
            "2024-04-30T05:00",
            "2024-04-30T06:00",
            "2024-04-30T07:00",
            "2024-04-30T08:00",
            "2024-04-30T09:00",
            "2024-04-30T10:00",
            "2024-04-30T11:00",
            "2024-04-30T12:00",
            "2024-04-30T13:00",
            "2024-04-30T14:00",
            "2024-04-30T15:00",
            "2024-04-30T16:00",
            "2024-04-30T17:00",
            "2024-04-30T18:00",
            "2024-04-30T19:00",
            "2024-04-30T20:00",
            "2024-04-30T21:00",
            "2024-04-30T22:00",
            "2024-04-30T23:00",
            "2024-05-01T00:00",
            "2024-05-01T01:00",
            "2024-05-01T02:00",
            "2024-05-01T03:00",
            "2024-05-01T04:00",
            "2024-05-01T05:00",
            "2024-05-01T06:00",
            "2024-05-01T07:00",
            "2024-05-01T08:00",
            "2024-05-01T09:00",
            "2024-05-01T10:00",
            "2024-05-01T11:00",
            "2024-05-01T12:00",
            "2024-05-01T13:00",
            "2024-05-01T14:00",
            "2024-05-01T15:00",
            "2024-05-01T16:00",
            "2024-05-01T17:00",
            "2024-05-01T18:00",
            "2024-05-01T19:00",
            "2024-05-01T20:00",
            "2024-05-01T21:00",
            "2024-05-01T22:00",
            "2024-05-01T23:00"
        ],
        "temperature_2m": [
            5.3,
            5.0,
            5.2,
            5.0,
            4.7,
            4.8,
            4.5,
            4.4,
            4.6,
            5.3,
            6.1,
            7.3,
            7.6,
            8.8,
            8.5,
            9.6,
            10.2,
            9.5,
            9.8,
            9.8,
            9.3,
            8.1,
            7.5,
            7.2,
            7.0,
            6.7,
            6.4,
            6.2,
            6.0,
            6.0,
            6.0,
            6.0,
            6.3,
            7.0,
            8.2,
            9.4,
            10.3,
            10.8,
            11.7,
            12.4,
            12.6,
            12.9,
            11.6,
            12.1,
            11.9,
            11.3,
            10.6,
            10.1,
            9.5,
            8.9,
            8.6,
            8.4,
            8.1,
            7.9,
            8.5,
            8.7,
            8.9,
            9.4,
            11.1,
            12.9,
            14.2,
            14.9,
            15.9,
            16.8,
            17.2,
            16.8,
            16.1,
            15.6,
            14.3,
            13.3,
            13.5,
            13.8,
            13.5,
            13.4,
            13.7,
            13.8,
            13.8,
            13.0,
            12.8,
            12.9,
            13.5,
            14.4,
            15.5,
            16.5,
            17.2,
            17.7,
            18.0,
            18.1,
            18.1,
            17.9,
            17.3,
            16.5,
            15.5,
            14.2,
            12.6,
            11.2,
            10.3,
            9.7,
            9.2,
            8.8,
            8.4,
            8.3,
            8.3,
            8.5,
            9.1,
            10.3,
            11.9,
            13.4,
            14.6,
            15.6,
            16.5,
            17.3,
            17.9,
            18.1,
            17.8,
            17.0,
            16.2,
            15.3,
            14.3,
            13.6,
            13.1,
            12.9,
            12.8,
            12.0,
            11.9,
            12.0,
            12.1,
            12.4,
            13.2,
            14.8,
            17.0,
            19.0,
            20.7,
            22.1,
            23.3,
            24.1,
            24.6,
            24.7,
            24.4,
            23.6,
            22.6,
            21.2,
            19.5,
            18.1,
            17.2,
            16.6,
            16.1,
            15.5,
            14.9,
            14.6,
            14.5,
            14.6,
            15.3,
            16.9,
            19.0,
            20.9,
            22.5,
            24.0,
            25.1,
            26.0,
            26.5,
            26.5,
            25.9,
            24.9,
            23.6,
            22.0,
            20.3,
            18.8
        ],
        "relative_humidity_2m": [
            85,
            87,
            88,
            88,
            88,
            89,
            89,
            88,
            86,
            81,
            77,
            68,
            65,
            61,
            61,
            55,
            50,
            58,
            52,
            56,
            58,
            71,
            75,
            77,
            78,
            79,
            81,
            81,
            81,
            79,
            79,
            79,
            77,
            77,
            74,
            73,
            68,
            68,
            66,
            62,
            64,
            64,
            78,
            73,
            73,
            79,
            85,
            86,
            85,
            86,
            86,
            89,
            90,
            89,
            83,
            85,
            87,
            89,
            81,
            75,
            70,
            71,
            66,
            61,
            58,
            60,
            61,
            64,
            76,
            86,
            82,
            69,
            69,
            67,
            65,
            64,
            65,
            71,
            73,
            73,
            70,
            66,
            60,
            56,
            54,
            52,
            51,
            50,
            50,
            50,
            51,
            53,
            56,
            62,
            69,
            76,
            81,
            84,
            87,
            89,
            90,
            91,
            91,
            91,
            88,
            82,
            75,
            68,
            62,
            57,
            53,
            50,
            47,
            47,
            49,
            54,
            59,
            65,
            71,
            76,
            79,
            82,
            83,
            87,
            89,
            89,
            88,
            86,
            83,
            77,
            69,
            62,
            57,
            52,
            49,
            47,
            47,
            48,
            51,
            55,
            60,
            67,
            75,
            81,
            84,
            86,
            88,
            90,
            91,
            92,
            92,
            92,
            90,
            85,
            78,
            71,
            66,
            62,
            58,
            53,
            47,
            45,
            47,
            52,
            57,
            63,
            69,
            74
        ],
        "weather_code": [
            2,
            3,
            3,
            61,
            61,
            61,
            61,
            3,
            61,
            3,
            3,
            3,
            3,
            3,
            61,
            3,
            3,
            61,
            3,
            3,
            3,
            61,
            61,
            3,
            3,
            61,
            61,
            61,
            61,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            61,
            61,
            3,
            3,
            3,
            61,
            61,
            3,
            3,
            3,
            61,
            3,
            2,
            3,
            3,
            2,
            2,
            3,
            3,
            3,
            61,
            61,
            3,
            2,
            2,
            2,
            3,
            3,
            3,
            80,
            61,
            61,
            61,
            61,
            61,
            61,
            61,
            61,
            3,
            3,
            3,
            2,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            3,
            3,
            3,
            2,
            2,
            2,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            3,
            2,
            2,
            2,
            3,
            3,
            3,
            2,
            2,
            2,
            2,
            2,
            2,
            1,
            1,
            1,
            2,
            2,
            2,
            3,
            3,
            3,
            1,
            1,
            1,
            3,
            3,
            3,
            1,
            1,
            1,
            3,
            3,
            3,
            1,
            1,
            1,
            1,
            1,
            1,
            2,
            2,
            2,
            2,
            2,
            2,
            1,
            1,
            1,
            1,
            1,
            1
        ]
    }
}
*/


/*
https://www.7timer.info/bin/api.pl?lon=6.7735&lat=51.2277&product=civil&output=json
{
    "product" : "civil" ,
    "init" : "2022101100" ,
    "dataseries" : [
    {
        "timepoint" : 3,
        "cloudcover" : 1,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 9,
        "rh2m" : "83%",
        "wind10m" : {
            "direction" : "NW",
            "speed" : 2
        },
        "weather" : "clearnight"
    },
    {
        "timepoint" : 6,
        "cloudcover" : 2,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 8,
        "rh2m" : "89%",
        "wind10m" : {
            "direction" : "NW",
            "speed" : 2
        },
        "weather" : "clearday"
    },
    {
        "timepoint" : 9,
        "cloudcover" : 8,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 11,
        "rh2m" : "69%",
        "wind10m" : {
            "direction" : "W",
            "speed" : 1
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 12,
        "cloudcover" : 8,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 14,
        "rh2m" : "54%",
        "wind10m" : {
            "direction" : "N",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 15,
        "cloudcover" : 2,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 14,
        "rh2m" : "41%",
        "wind10m" : {
            "direction" : "NE",
            "speed" : 2
        },
        "weather" : "clearday"
    },
    {
        "timepoint" : 18,
        "cloudcover" : 2,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 11,
        "rh2m" : "64%",
        "wind10m" : {
            "direction" : "NE",
            "speed" : 2
        },
        "weather" : "clearnight"
    },
    {
        "timepoint" : 21,
        "cloudcover" : 1,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 10,
        "rh2m" : "65%",
        "wind10m" : {
            "direction" : "E",
            "speed" : 2
        },
        "weather" : "clearnight"
    },
    {
        "timepoint" : 24,
        "cloudcover" : 2,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 9,
        "rh2m" : "74%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 2
        },
        "weather" : "clearnight"
    },
    {
        "timepoint" : 27,
        "cloudcover" : 5,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 8,
        "rh2m" : "76%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 2
        },
        "weather" : "pcloudynight"
    },
    {
        "timepoint" : 30,
        "cloudcover" : 7,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 8,
        "rh2m" : "78%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 2
        },
        "weather" : "mcloudyday"
    },
    {
        "timepoint" : 33,
        "cloudcover" : 9,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 11,
        "rh2m" : "58%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 36,
        "cloudcover" : 8,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 15,
        "rh2m" : "44%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 39,
        "cloudcover" : 2,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 15,
        "rh2m" : "40%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "clearday"
    },
    {
        "timepoint" : 42,
        "cloudcover" : 5,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 13,
        "rh2m" : "60%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "pcloudynight"
    },
    {
        "timepoint" : 45,
        "cloudcover" : 8,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 11,
        "rh2m" : "63%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "cloudynight"
    },
    {
        "timepoint" : 48,
        "cloudcover" : 9,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 10,
        "rh2m" : "74%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudynight"
    },
    {
        "timepoint" : 51,
        "cloudcover" : 9,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 10,
        "rh2m" : "79%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudynight"
    },
    {
        "timepoint" : 54,
        "cloudcover" : 9,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 9,
        "rh2m" : "83%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 57,
        "cloudcover" : 9,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 0,
        "temp2m" : 11,
        "rh2m" : "64%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 60,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 1,
        "temp2m" : 11,
        "rh2m" : "65%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 63,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 1,
        "temp2m" : 11,
        "rh2m" : "85%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 66,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 1,
        "temp2m" : 11,
        "rh2m" : "88%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudynight"
    },
    {
        "timepoint" : 69,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "rain",
        "prec_amount" : 2,
        "temp2m" : 11,
        "rh2m" : "89%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 2
        },
        "weather" : "lightrainnight"
    },
    {
        "timepoint" : 72,
        "cloudcover" : 9,
        "lifted_index" : 10,
        "prec_type" : "rain",
        "prec_amount" : 2,
        "temp2m" : 11,
        "rh2m" : "91%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "lightrainnight"
    },
    {
        "timepoint" : 75,
        "cloudcover" : 9,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 2,
        "temp2m" : 10,
        "rh2m" : "93%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 3
        },
        "weather" : "cloudynight"
    },
    {
        "timepoint" : 78,
        "cloudcover" : 9,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 2,
        "temp2m" : 10,
        "rh2m" : "86%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 3
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 81,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "rain",
        "prec_amount" : 3,
        "temp2m" : 11,
        "rh2m" : "95%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 3
        },
        "weather" : "lightrainday"
    },
    {
        "timepoint" : 84,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "rain",
        "prec_amount" : 3,
        "temp2m" : 11,
        "rh2m" : "96%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 3
        },
        "weather" : "lightrainday"
    },
    {
        "timepoint" : 87,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "rain",
        "prec_amount" : 3,
        "temp2m" : 12,
        "rh2m" : "99%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "lightrainday"
    },
    {
        "timepoint" : 90,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 3,
        "temp2m" : 12,
        "rh2m" : "98%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "lightrainnight"
    },
    {
        "timepoint" : 93,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "rain",
        "prec_amount" : 3,
        "temp2m" : 12,
        "rh2m" : "97%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "lightrainnight"
    },
    {
        "timepoint" : 96,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "rain",
        "prec_amount" : 3,
        "temp2m" : 11,
        "rh2m" : "97%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "lightrainnight"
    },
    {
        "timepoint" : 99,
        "cloudcover" : 7,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 3,
        "temp2m" : 10,
        "rh2m" : "97%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "mcloudynight"
    },
    {
        "timepoint" : 102,
        "cloudcover" : 6,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 3,
        "temp2m" : 10,
        "rh2m" : "96%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 2
        },
        "weather" : "mcloudyday"
    },
    {
        "timepoint" : 105,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 3,
        "temp2m" : 13,
        "rh2m" : "91%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 3
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 108,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "none",
        "prec_amount" : 3,
        "temp2m" : 14,
        "rh2m" : "89%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 3
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 111,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 3,
        "temp2m" : 14,
        "rh2m" : "85%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "lightrainday"
    },
    {
        "timepoint" : 114,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 3,
        "temp2m" : 14,
        "rh2m" : "92%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 3
        },
        "weather" : "lightrainnight"
    },
    {
        "timepoint" : 117,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 13,
        "rh2m" : "88%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 3
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 120,
        "cloudcover" : 8,
        "lifted_index" : 6,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 11,
        "rh2m" : "81%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 3
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 123,
        "cloudcover" : 6,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 11,
        "rh2m" : "85%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 3
        },
        "weather" : "mcloudynight"
    },
    {
        "timepoint" : 126,
        "cloudcover" : 7,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 10,
        "rh2m" : "75%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 3
        },
        "weather" : "mcloudyday"
    },
    {
        "timepoint" : 129,
        "cloudcover" : 4,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 12,
        "rh2m" : "60%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 3
        },
        "weather" : "pcloudyday"
    },
    {
        "timepoint" : 132,
        "cloudcover" : 6,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 15,
        "rh2m" : "50%",
        "wind10m" : {
            "direction" : "W",
            "speed" : 3
        },
        "weather" : "mcloudyday"
    },
    {
        "timepoint" : 135,
        "cloudcover" : 8,
        "lifted_index" : 6,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 15,
        "rh2m" : "55%",
        "wind10m" : {
            "direction" : "W",
            "speed" : 3
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 138,
        "cloudcover" : 7,
        "lifted_index" : 10,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 12,
        "rh2m" : "68%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "mcloudynight"
    },
    {
        "timepoint" : 141,
        "cloudcover" : 8,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 11,
        "rh2m" : "75%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 2
        },
        "weather" : "cloudynight"
    },
    {
        "timepoint" : 144,
        "cloudcover" : 8,
        "lifted_index" : 15,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 10,
        "rh2m" : "74%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 2
        },
        "weather" : "cloudynight"
    },
    {
        "timepoint" : 147,
        "cloudcover" : 9,
        "lifted_index" : 15,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 11,
        "rh2m" : "70%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 3
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 150,
        "cloudcover" : 9,
        "lifted_index" : 10,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 12,
        "rh2m" : "83%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 3
        },
        "weather" : "rainday"
    },
    {
        "timepoint" : 153,
        "cloudcover" : 9,
        "lifted_index" : 6,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 15,
        "rh2m" : "79%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 3
        },
        "weather" : "rainday"
    },
    {
        "timepoint" : 156,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 20,
        "rh2m" : "61%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 3
        },
        "weather" : "rainday"
    },
    {
        "timepoint" : 159,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 17,
        "rh2m" : "65%",
        "wind10m" : {
            "direction" : "SE",
            "speed" : 2
        },
        "weather" : "rainday"
    },
    {
        "timepoint" : 162,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 16,
        "rh2m" : "88%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 165,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 15,
        "rh2m" : "97%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 168,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 14,
        "rh2m" : "98%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 171,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 14,
        "rh2m" : "98%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 174,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 14,
        "rh2m" : "97%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 177,
        "cloudcover" : 8,
        "lifted_index" : 2,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 16,
        "rh2m" : "90%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 180,
        "cloudcover" : 8,
        "lifted_index" : 2,
        "prec_type" : "none",
        "prec_amount" : 4,
        "temp2m" : 18,
        "rh2m" : "74%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "cloudyday"
    },
    {
        "timepoint" : 183,
        "cloudcover" : 9,
        "lifted_index" : -1,
        "prec_type" : "rain",
        "prec_amount" : 4,
        "temp2m" : 18,
        "rh2m" : "70%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "rainday"
    },
    {
        "timepoint" : 186,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 5,
        "temp2m" : 16,
        "rh2m" : "92%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 189,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 5,
        "temp2m" : 15,
        "rh2m" : "91%",
        "wind10m" : {
            "direction" : "S",
            "speed" : 2
        },
        "weather" : "rainnight"
    },
    {
        "timepoint" : 192,
        "cloudcover" : 9,
        "lifted_index" : 2,
        "prec_type" : "rain",
        "prec_amount" : 5,
        "temp2m" : 14,
        "rh2m" : "93%",
        "wind10m" : {
            "direction" : "SW",
            "speed" : 2
        },
        "weather" : "rainnight"
    }
    ]
}
*/

