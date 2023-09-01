using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Vishnu.Interchange;

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
    /// The weather forecasts come from http://www.7timer.info.
    /// Many thanks to Chenzhou Cui and his friends, who run this wonderful free weather-forecast site.
    /// Wiki on https://github.com/Yeqzids/7timer-issues/wiki/Wiki.
    /// API on http://www.7timer.info/bin/api.pl?lon=6.7821&amp;lat=51.2375&amp;product=civil&amp;output=json.
    /// Further I want to thank the developer-team who runs https://ip-api.com/ which provides a geolocation service,
    /// that is free for non commercial usage.
    /// 
    /// 11.10.2020 Erik Nagel: erstellt
    /// </remarks>
    public class WeatherChecker : INodeChecker, IDisposable
    {
        #region INodeChecker Implementation

        /// <summary>
        /// Kann aufgerufen werden, wenn sich der Verarbeitungs-Fortschritt
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

        private async Task<GeoLocation_ReturnObject?> GetGeoLocation()
        {
            Uri uri = new Uri(@"http://ip-api.com/json");
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10);
            HttpResponseMessage? response = null;
            string? responseJsonString = null;
            GeoLocation_ReturnObject? deserializedLocation = null;
            try
            {
                response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                responseJsonString = await response.Content.ReadAsStringAsync();
                deserializedLocation = JsonConvert.DeserializeObject<GeoLocation_ReturnObject>(responseJsonString);
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

            return deserializedLocation;
        }

        private async Task<bool?> Work(TreeEvent source)
        {
            string? lon = "6.7821";  // fallback
            string? lat = "51.2375"; // = Düsseldorf

            GeoLocation_ReturnObject? geoLocation_ReturnObject = Task.Run(() => this.GetGeoLocation()).Result;

            if (geoLocation_ReturnObject != null)
            {
                lon = geoLocation_ReturnObject.lon.ToString()?.Replace(',', '.');
                lat = geoLocation_ReturnObject.lat.ToString()?.Replace(',', '.');

            }

            // Hier folgt die eigentliche Checker-Verarbeitung, die einen erweiterten boolean als Rückgabe
            // dieses Checkers ermittelt und das WeatherChecker_ReturnObject mit zusätzlichen Informationen
            // aus dem nachfolgenden API-Aufruf der Seite www.7timer.info füllt.

            Uri uri = new Uri(@"http://www.7timer.info/bin/api.pl?lon=" + lon + "&lat=" + lat + "&product=civil&output=json");

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10);
            HttpResponseMessage? response = null;
            string? responseJsonString = null;
            WeatherChecker_ReturnObject? deserializedWeather = null;
            try
            {
                response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                responseJsonString = await response.Content.ReadAsStringAsync();
                deserializedWeather = JsonConvert.DeserializeObject<WeatherChecker_ReturnObject>(responseJsonString);
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
            if (deserializedWeather != null && geoLocation_ReturnObject != null)
            {
                deserializedWeather.Location = geoLocation_ReturnObject;
            }
            this._returnObject = deserializedWeather;
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
// So sieht das Ergebnis des API-Calls in der Rohfassung aus:
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

