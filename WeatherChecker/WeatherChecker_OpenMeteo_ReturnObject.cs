using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WeatherChecker
{
    /// <summary>
    /// This class defines the units used in the hourly data.
    /// </summary>
    [DataContract]
    public class HourlyUnits
    {
        /// <summary>
        /// The unit of time used for the timestamps in the hourly data (e.g., "ISO8601").
        /// </summary>
        [DataMember]
        public string? Time { get; set; }

        /// <summary>
        /// The unit of measurement for the 2-meter temperature data (e.g., "celsius").
        /// </summary>
        [DataMember(Name = "temperature_2m")]
        public string? Temperature2m { get; set; }

        /// <summary>
        /// The unit of measurement for the relative humidity at 2 meters data (e.g., "%").
        /// </summary>
        [DataMember(Name = "relative_humidity_2m")]
        public string? RelativeHumidity2m { get; set; }

        /// <summary>
        /// The unit of measurement for the weather code data.
        /// </summary>
        [DataMember(Name = "weather_code")]
        public string? WeatherCode { get; set; }

        /// <summary>
        /// Standard constructor, necessary for json-deserialization.
        /// </summary>
        public HourlyUnits()
        {
        }
    }

    /// <summary>
    /// This class represents the hourly weather data.
    /// </summary>
    [DataContract]
    public class Hourly
    {
        /// <summary>
        /// List of timestamps for each data point (same unit as specified in HourlyUnits.Time).
        /// </summary>
        [DataMember]
        public List<string>? Time { get; set; }

        /// <summary>
        /// List of 2-meter temperature values in the specified unit (from HourlyUnits.Temperature2m).
        /// </summary>
        [DataMember(Name = "temperature_2m")]
        public List<double>? Temperature2m { get; set; }

        /// <summary>
        /// List of relative humidity values at 2 meters (in the unit specified by HourlyUnits.RelativeHumidity2m).
        /// </summary>
        [DataMember(Name = "relative_humidity_2m")]
        public List<int>? RelativeHumidity2m { get; set; }

        /// <summary>
        /// List of weather codes for each data point.
        /// </summary>
        [DataMember(Name = "weather_code")]
        public List<int>? WeatherCode { get; set; }

        /// <summary>
        /// Standard constructor, necessary for json-deserialization.
        /// </summary>
        public Hourly()
        {
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        /// <param name="info">Property container.</param>
        /// <param name="context">Transscription context.</param>
        protected Hourly(SerializationInfo info, StreamingContext context)
        {
            Time = (List<string>?)info.GetValue("Time", typeof(List<string>));
            Temperature2m = (List<double>?)info.GetValue("Temperature2m", typeof(List<double>));
            RelativeHumidity2m = (List<int>?)info.GetValue("RelativeHumidity2m", typeof(List<int>));
            WeatherCode = (List<int>?)info.GetValue("WeatherCode", typeof(List<int>));
        }

        /// <summary>
        /// Serialization method.
        /// </summary>
        /// <param name="info">Property container.</param>
        /// <param name="context">Transscription context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Time", Time);
            info.AddValue("Temperature2m", Temperature2m);
            info.AddValue("RelativeHumidity2m", RelativeHumidity2m);
            info.AddValue("WeatherCode", WeatherCode);
        }

        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>This object.ToString().</returns>
        public override string ToString()
        {
            return $"Time: {string.Join(", ", Time?? new List<string>())}, Temperature2m: {string.Join(", ", Temperature2m?? new List<double>())}"
                 + $", RelativeHumidity2m: {string.Join(", ", RelativeHumidity2m?? new List<int>())}, WeatherCode: {string.Join(", ", WeatherCode?? new List<int>())}";
        }

        /// <summary>
        /// Vergleicht dieses Objekt mit einem übergebenen Objekt nach Inhalt.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True, wenn das übergebene Objekt inhaltlich gleich diesem Objekt ist.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (this.ToString() != obj.ToString())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Erzeugt einen eindeutigen Hashcode für dieses Objekt.
        /// </summary>
        /// <returns>Hashcode (int).</returns>
        public override int GetHashCode()
        {
            return (this.ToString()).GetHashCode();
        }

    }

    /// <summary>
    /// This class defines the units used in the daily data.
    /// </summary>
    [DataContract]
    public class DailyUnits
    {
        /// <summary>
        /// The unit of time used for the timestamps in the daily data (e.g., "ISO8601").
        /// </summary>
        [DataMember]
        public string? Time { get; set; }

        /// <summary>
        /// The unit of time used for sunrise and sunset times (e.g., "ISO8601").
        /// </summary>
        [DataMember]
        public string? Sunrise { get; set; }

        /// <summary>
        /// The unit of time used for sunrise and sunset times (e.g., "ISO8601").
        /// </summary>
        [DataMember]
        public string? Sunset { get; set; }

        /// <summary>
        /// Standard constructor, necessary for json-deserialization.
        /// </summary>
        public DailyUnits()
        {
        }
    }

    /// <summary>
    /// This class represents the daily weather data.
    /// </summary>
    [DataContract]
    public class Daily
    {
        /// <summary>
        /// List of timestamps for each data point (same unit as specified in DailyUnits.Time).
        /// </summary>
        [DataMember]
        public List<string>? Time { get; set; }

        /// <summary>
        /// List of sunrise times for each day (in the unit specified by DailyUnits.Sunrise).
        /// </summary>
        [DataMember]
        public List<string>? Sunrise { get; set; }

        /// <summary>
        /// List of sunset times for each day (in the unit specified by DailyUnits.Sunset).
        /// </summary>
        [DataMember]
        public List<string>? Sunset { get; set; }

        /// <summary>
        /// Standard constructor, necessary for json-deserialization.
        /// </summary>
        public Daily()
        {
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        /// <param name="info">Property container.</param>
        /// <param name="context">Transscription context.</param>
        protected Daily(SerializationInfo info, StreamingContext context)
        {
            Time = (List<string>?)info.GetValue("Time", typeof(List<string>));
            Sunrise = (List<string>?)info.GetValue("Sunrise", typeof(List<string>));
            Sunset = (List<string>?)info.GetValue("Sunset", typeof(List<string>));
        }

        /// <summary>
        /// Serialization method.
        /// </summary>
        /// <param name="info">Property container.</param>
        /// <param name="context">Transscription context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Time", Time);
            info.AddValue("Sunrise", Sunrise);
            info.AddValue("Sunset", Sunset);
        }

        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>This object.ToString().</returns>
        public override string ToString()
        {
            return $"Time: {string.Join(", ", Time?? new List<string>())}, Sunrise: {string.Join(", ", Sunrise?? new List<string>())}"
                + $", Sunset: {string.Join(", ", Sunset ?? new List<string>())}";
        }

        /// <summary>
        /// Vergleicht dieses Objekt mit einem übergebenen Objekt nach Inhalt.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True, wenn das übergebene Objekt inhaltlich gleich diesem Objekt ist.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (this.ToString() != obj.ToString())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Erzeugt einen eindeutigen Hashcode für dieses Objekt.
        /// </summary>
        /// <returns>Hashcode (int).</returns>
        public override int GetHashCode()
        {
            return (this.ToString()).GetHashCode();
        }

    }

    /// <summary>
    /// This class represents the root object of the OpenWeatherMap API response.
    /// </summary>
    [DataContract]
    public class WeatherChecker_OpenMeteo_ReturnObject
    {
        /// <summary>
        /// Geographic latitude of the requested location.
        /// </summary>
        [DataMember]
        public double? Latitude { get; set; }

        /// <summary>
        /// Geographic longitude of the requested location.
        /// </summary>
        [DataMember]
        public double? Longitude { get; set; }

        /// <summary>
        /// Time taken to generate the response in milliseconds.
        /// </summary>
        [DataMember(Name = "generationtime_ms")]
        public double? GenerationTimeMs { get; set; }

        /// <summary>
        /// Offset from UTC time in seconds.
        /// </summary>
        [DataMember(Name = "utc_offset_seconds")]
        public int? UtcOffsetSeconds { get; set; }

        /// <summary>
        /// Timezone name for the requested location.
        /// </summary>
        [DataMember]
        public string? Timezone { get; set; }

        /// <summary>
        /// Abbreviated timezone name for the requested location.
        /// </summary>
        [DataMember(Name = "timezone_abbreviation")]
        public string? TimezoneAbbreviation { get; set; }

        /// <summary>
        /// Elevation of the requested location (in meters).
        /// </summary>
        [DataMember]
        public double? Elevation { get; set; }

        /// <summary>
        /// Hourly weather data.
        /// </summary>
        [DataMember(Name = "hourly_units")]
        public HourlyUnits? HourlyUnits { get; set; }

        /// <summary>
        /// Hourly weather data points.
        /// </summary>
        [DataMember]
        public Hourly? Hourly { get; set; }

        /// <summary>
        /// Daily data units.
        /// </summary>
        [DataMember(Name = "daily_units")]
        public DailyUnits? DailyUnits { get; set; }

        /// <summary>
        /// Daily weather data points.
        /// </summary>
        [DataMember]
        public Daily? Daily { get; set; }

        /// <summary>
        /// Standard constructor, necessary for json-deserialization.
        /// </summary>
        public WeatherChecker_OpenMeteo_ReturnObject()
        {
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        /// <param name="info">Property container.</param>
        /// <param name="context">Transscription context.</param>
        protected WeatherChecker_OpenMeteo_ReturnObject(SerializationInfo info, StreamingContext context)
        {
            Latitude = (double?)info.GetValue("Latitude", typeof(double));
            Longitude = (double?)info.GetValue("Longitude", typeof(double));
            GenerationTimeMs = (double?)info.GetValue("GenerationTimeMs", typeof(double));
            UtcOffsetSeconds = (int?)info.GetValue("UTCOffsetSeconds", typeof(int));
            Timezone = (string?)info.GetValue("Timezone", typeof(string));
            TimezoneAbbreviation = (string?)info.GetValue("TimezoneAbbreviation", typeof(string));
            Elevation = (double?)info.GetValue("Elevation", typeof(double));
            HourlyUnits = (HourlyUnits?)info.GetValue("HourlyUnits", typeof(HourlyUnits));
            Hourly = (Hourly?)info.GetValue("Hourly", typeof(Hourly));
            DailyUnits = (DailyUnits?)info.GetValue("DailyUnits", typeof(Dictionary<string, string>));
            Daily = (Daily?)info.GetValue("Daily", typeof(Daily));
        }

        /// <summary>
        /// Serialization method.
        /// </summary>
        /// <param name="info">Property container.</param>
        /// <param name="context">Transscription context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Latitude", Latitude);
            info.AddValue("Longitude", Longitude);
            info.AddValue("GenerationTimeMs", GenerationTimeMs);
            info.AddValue("UTCOffsetSeconds", UtcOffsetSeconds);
            info.AddValue("Timezone", Timezone);
            info.AddValue("TimezoneAbbreviation", TimezoneAbbreviation);
            info.AddValue("Elevation", Elevation);
            info.AddValue("HourlyUnits", HourlyUnits);
            info.AddValue("Hourly", Hourly);
            info.AddValue("DailyUnits", DailyUnits);
            info.AddValue("Daily", Daily);
        }

        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>This object.ToString().</returns>
        public override string ToString()
        {
            return $"Latitude: {Latitude}, Longitude: {Longitude}, GenerationTimeMs: {GenerationTimeMs}"
                + $", UTCOffsetSeconds: {UtcOffsetSeconds}, Timezone: {Timezone}, TimezoneAbbreviation: {TimezoneAbbreviation}"
                + $", Elevation: {Elevation}, HourlyUnits: {HourlyUnits}, Hourly: {Hourly}, DailyUnits: {DailyUnits}, Daily: {Daily}";
        }

        /// <summary>
        /// Vergleicht dieses Objekt mit einem übergebenen Objekt nach Inhalt.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True, wenn das übergebene Objekt inhaltlich gleich diesem Objekt ist.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (this.ToString() != obj.ToString())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Erzeugt einen eindeutigen Hashcode für dieses Objekt.
        /// </summary>
        /// <returns>Hashcode (int).</returns>
        public override int GetHashCode()
        {
            return (this.ToString()).GetHashCode();
        }

        internal static WeatherChecker_ReturnObject? ConvertToWeatherChecker_ReturnObject(WeatherChecker_OpenMeteo_ReturnObject? deserializedWeather, GeoLocation_ReturnObject? geoLocation_ReturnObject)
        {
            WeatherChecker_ReturnObject? weatherChecker_ReturnObject = new WeatherChecker_ReturnObject();
            if (geoLocation_ReturnObject == null) return weatherChecker_ReturnObject;
            weatherChecker_ReturnObject.Location = geoLocation_ReturnObject;
            if (deserializedWeather == null || deserializedWeather.Hourly == null || deserializedWeather.Daily == null) return weatherChecker_ReturnObject;
            for (int i = 0; i < deserializedWeather.Hourly.Time?.Count; i+=3)
            {
                string timeString = deserializedWeather.Hourly.Time[i];
                WeatherChecker_ReturnObject.ForecastDataPoint forecastDataPoint = new WeatherChecker_ReturnObject.ForecastDataPoint();
                forecastDataPoint.Timepoint = timeString;
                weatherChecker_ReturnObject.Dataseries?.Add(forecastDataPoint);
            }
            for (int i = 0; i < weatherChecker_ReturnObject.Dataseries?.Count; i++)
            {
                int j = i * 3;
                string timePoint = weatherChecker_ReturnObject.Dataseries[i].Timepoint ?? throw new ArgumentNullException("Timepoint");
                ConversionHelpers.FetchSunriseAndSunset(deserializedWeather, timePoint, out string sunriseString, out string sunsetString);
                weatherChecker_ReturnObject.Dataseries[i].Weather
                    = ConversionHelpers.WMONametoSimplifiedWeatherNameWithDayOrNight(deserializedWeather.Hourly.WeatherCode?[j] ?? 0,
                    timePoint, sunriseString, sunsetString);

                weatherChecker_ReturnObject.Dataseries[i].Temperature = deserializedWeather.Hourly.Temperature2m?[j].ToString() + "°C"; ;
                weatherChecker_ReturnObject.Dataseries[i].Humidity = deserializedWeather.Hourly.RelativeHumidity2m?[j].ToString() + "%";
            }
            /*
                // this.Timepoint = info.GetString("Timepoint");
                this.Cloudcover = (int?)info.GetValue("Cloudcover", typeof(int));
                this.Lifted_Index = (int?)info.GetValue("Lifted_Index", typeof(int));
                this.Prec_Type = info.GetString("Prec_Type");
                this.Prec_Amount = (int?)info.GetValue("Prec_Amount", typeof(int));
                this.Temp2m = (int?)info.GetValue("Temp2m", typeof(int));
                this.Rh2m = info.GetString("Rh2m");
                this.Wind10m = (Wind?)info.GetValue("Wind10m", typeof(Wind));
                // this.Weather = info.GetString("Weather");
            */
            return weatherChecker_ReturnObject;
        }

    }

    static class ConversionHelpers
    {
        internal static string WMONametoSimplifiedWeatherNameWithDayOrNight(int wmoCode, string dateTimeString, string sunriseString, string sunsetString /* "2024-04-29T09:00" */)
        {
            //return "Harry";
            string wmoName = WmoWeatherName[wmoCode];
            if (IsDayLight(dateTimeString, sunriseString, sunsetString))
            {
                return wmoName + "day";
            }
            else
            {
                return wmoName + "night";
            }
        }

        internal static void FetchSunriseAndSunset(WeatherChecker_OpenMeteo_ReturnObject deserializedWeather,
            string? dateTimeString, out string sunriseString, out string sunsetString)
        {
            string dayString = dateTimeString?.Substring(0, 10) ?? throw new ArgumentNullException();
            sunriseString = "";
            sunsetString = "";
            for (int i = 0; i < deserializedWeather.Daily?.Time?.Count; i++)
            {
                if (deserializedWeather.Daily?.Time[i] == dayString)
                {
                    sunriseString = deserializedWeather.Daily?.Sunrise?[i] ?? "";
                    sunsetString = deserializedWeather.Daily?.Sunset?[i] ?? "";
                    break;
                }
            }
        }

        internal static bool IsDayLight(string dateTimeString, string sunriseString, string sunsetString /* "2024-04-29T09:00" */)
        {
            return dateTimeString.CompareTo(sunriseString) >= 0 && dateTimeString.CompareTo(sunsetString) < 0;
        }

        internal static string WMONametoSimplifiedWeatherNameWithDayOrNight(int wmoCode, int latitude, string dateTimeString /* "2024-04-29T09:00" */)
        {
            string wmoName = WmoWeatherName[wmoCode];
            if (IsDayLight(wmoCode, latitude, dateTimeString))
            {
                return wmoName + "day";
            }
            else
            {
                return wmoName + "night";
            }
        }

        internal static int DateTimeToQuarter(DateTime dateTime)
        {
            int month = dateTime.Month;
            int day = dateTime.Day;
            int monthDay = month * 100 + day;
            if (monthDay > 200 && monthDay < 500)
            {
                return 1;
            }
            if (monthDay > 500 && monthDay < 800)
            {
                return 2;
            }
            if (monthDay > 800 && monthDay < 1100)
            {
                return 3;
            }
            return 4;
        }

        /// <summary>
        /// This dictionary maps WMO weather codes to simplified weather names.
        /// </summary>
        internal static readonly Dictionary<int, string> WmoWeatherName
            = new Dictionary<int, string>()
            {
                {0, "clear"},
                {1, "clear"},
                {2, "pcloudy"},
                {3, "mcloudy"},
                {45, "humid"},
                {48, "humid"},
                {51, "ishower"},
                {53, "ishower"},
                {55, "ishower"},
                {56, "ishower"},
                {57, "ishower"},
                {61, "lightrain"},
                {63, "rain"},
                {65, "rain"},
                {66, "rainsnow"},
                {67, "rain"},
                {71, "lightsnow"},
                {73, "snow"},
                {75, "snow"},
                {77, "snow"},
                {80, "oshower"},
                {81, "oshower"},
                {82, "oshower"},
                {85, "lightsnow"},
                {86, "snow"},
                {95, "ts"},
                {96, "tsrain"},
                {99, "tsrain"},
                {100, "windy"}
            };


        /*
        internal static readonly Dictionary<string, string> _wmoWeatherName
            = new Dictionary<string, string>()
            {
                {"clearday", "clearday"},
                {"", "clearday"},
                {"clearnight", "clearnight"},
                {"cloudyday", "cloudyday"},
                {"cloudynight", "cloudynight"},
                {"humidday", "humidday"},
                {"humidnight", "humidnight"},
                {"ishowerday", "ishowerday"},
                {"ishowernight", "ishowernight"},
                {"lightrainday", "lightrainday"},
                {"lightrainnight", "lightrainnight"},
                {"lightsnowday", "lightsnowday"},
                {"lightsnownight", "lightsnownight"},
                {"mcloudyday", "mcloudyday"},
                {"mcloudynight", "mcloudynight"},
                {"oshowerday", "oshowerday"},
                {"oshowernight", "oshowernight"},
                {"pcloudyday", "pcloudyday"},
                {"pcloudynight", "pcloudynight"},
                {"rainday", "rainday"},
                {"rainnight", "rainnight"},
                {"rainsnowday", "rainsnowday"},
                {"rainsnownight", "rainsnownight"},
                {"snowday", "snowday"},
                {"snownight", "snownight"},
                {"tsday", "tsday"},
                {"tsnight", "tsnight"},
                {"tsrainday", "tsrainday"},
                {"tsrainnight", "tsrainnight"},
                {"windy", "windy"}
            };
        */

        internal static bool IsDayLight(int wmoCode, int latitude, string dateTimeString /* "2024-04-29T09:00" */)
        {
            DateTime dateTime = DateTime.Parse(dateTimeString);
            int quarter = DateTimeToQuarter(dateTime);
            TimeSpan daylightFrom = _quarterLatitudeTime[quarter][latitude][0];
            TimeSpan daylightTo = _quarterLatitudeTime[quarter][latitude][1];
            TimeSpan timeNow = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
            return (daylightFrom < daylightTo && timeNow >= daylightFrom && timeNow < daylightTo);
        }

        internal static readonly Dictionary<int, TimeSpan[]>[] _quarterLatitudeTime = new Dictionary<int, TimeSpan[]>[]
          {
            new Dictionary<int, TimeSpan[]> // Q1
             {
                   { 90, new TimeSpan[] { TimeSpan.Parse("23:59:59.999"), TimeSpan.Parse("00:00") } }
                 , { 80, new TimeSpan[] { TimeSpan.Parse("06:33"), TimeSpan.Parse("17:43") } }
                 , { 60, new TimeSpan[] { TimeSpan.Parse("06:32"), TimeSpan.Parse("18:20") } }
                 , { 40, new TimeSpan[] { TimeSpan.Parse("07:52"), TimeSpan.Parse("19:22") } }
                 , { 20, new TimeSpan[] { TimeSpan.Parse("06:37"), TimeSpan.Parse("18:38") } }
                 , { 0, new TimeSpan[] { TimeSpan.Parse("06:00"), TimeSpan.Parse("18:00") } }
                 , { -20, new TimeSpan[] { TimeSpan.Parse("05:43"), TimeSpan.Parse("17:55") } }
                 , { -40, new TimeSpan[] { TimeSpan.Parse("07:51"), TimeSpan.Parse("20:11") } }
                 , { -60, new TimeSpan[] { TimeSpan.Parse("07:25"), TimeSpan.Parse("19:57") } }
                 , { -80, new TimeSpan[] { TimeSpan.Parse("07:18"), TimeSpan.Parse("20:59") } }
                 , { -90, new TimeSpan[] { TimeSpan.Parse("00:00"), TimeSpan.Parse("23:59:59.999") } }
             },
            new Dictionary<int, TimeSpan[]> // Q2
             {
                  { 90, new TimeSpan[] { TimeSpan.Parse("00:00"), TimeSpan.Parse("23:59:59.999") } }
                , { 80, new TimeSpan[] { TimeSpan.Parse("00:00"), TimeSpan.Parse("23:59:59.999") } }
                , { 60, new TimeSpan[] { TimeSpan.Parse("03:54"), TimeSpan.Parse("22:41") } }
                , { 40, new TimeSpan[] { TimeSpan.Parse("06:44"), TimeSpan.Parse("21:46") } }
                , { 20, new TimeSpan[] { TimeSpan.Parse("05:50"), TimeSpan.Parse("19:08") } }
                , { 0, new TimeSpan[] { TimeSpan.Parse("06:00"), TimeSpan.Parse("18:00") } }
                , { -20, new TimeSpan[] { TimeSpan.Parse("06:13"), TimeSpan.Parse("17:09") } }
                , { -40, new TimeSpan[] { TimeSpan.Parse("08:12"), TimeSpan.Parse("17:34") } }
                , { -60, new TimeSpan[] { TimeSpan.Parse("09:56"), TimeSpan.Parse("17:10") } }
                , { -80, new TimeSpan[] { TimeSpan.Parse("23:59:59.999"), TimeSpan.Parse("00:00") } }
                , { -90, new TimeSpan[] { TimeSpan.Parse("23:59:59.999"), TimeSpan.Parse("00:00") } }
            },
            new Dictionary<int, TimeSpan[]> // Q3
            {
                { 90, new TimeSpan[] { TimeSpan.Parse("00:00"), TimeSpan.Parse("23:59:59.999") } }
                , { 80, new TimeSpan[] { TimeSpan.Parse("05:34"), TimeSpan.Parse("20:06") } }
                , { 60, new TimeSpan[] { TimeSpan.Parse("06:45"), TimeSpan.Parse("19:36") } }
                , { 40, new TimeSpan[] { TimeSpan.Parse("07:55"), TimeSpan.Parse("20:23") } }
                , { 20, new TimeSpan[] { TimeSpan.Parse("06:16"), TimeSpan.Parse("18:31") } }
                , { 0, new TimeSpan[] { TimeSpan.Parse("06:00"), TimeSpan.Parse("18:00") } }
                , { -20, new TimeSpan[] { TimeSpan.Parse("05:36"), TimeSpan.Parse("17:35") } }
                , { -40, new TimeSpan[] { TimeSpan.Parse("07:52"), TimeSpan.Parse("19:43") } }
                , { -60, new TimeSpan[] { TimeSpan.Parse("07:38"), TimeSpan.Parse("19:19") } }
                , { -80, new TimeSpan[] { TimeSpan.Parse("08:54"), TimeSpan.Parse("18:55") } }
                , { -90, new TimeSpan[] { TimeSpan.Parse("23:59:59.999"), TimeSpan.Parse("00:00") } }
            },
            new Dictionary<int, TimeSpan[]> // Q4
            {
                  { 90, new TimeSpan[] { TimeSpan.Parse("23:59:59.999"), TimeSpan.Parse("00:00") } }
                , { 80, new TimeSpan[] { TimeSpan.Parse("23:59:59.999"), TimeSpan.Parse("00:00") } }
                , { 60, new TimeSpan[] { TimeSpan.Parse("09:13"), TimeSpan.Parse("15:10") } }
                , { 40, new TimeSpan[] { TimeSpan.Parse("08:30"), TimeSpan.Parse("17:49") } }
                , { 20, new TimeSpan[] { TimeSpan.Parse("06:55"), TimeSpan.Parse("17:53") } }
                , { 0, new TimeSpan[] { TimeSpan.Parse("06:00"), TimeSpan.Parse("18:00") } }
                , { -20, new TimeSpan[] { TimeSpan.Parse("06:56"), TimeSpan.Parse("18:15") } }
                , { -40, new TimeSpan[] { TimeSpan.Parse("06:19"), TimeSpan.Parse("21:17") } }
                , { -60, new TimeSpan[] { TimeSpan.Parse("04:49"), TimeSpan.Parse("22:07") } }
                , { -80, new TimeSpan[] { TimeSpan.Parse("00:00"), TimeSpan.Parse("23:59:59.999") } }
                , { -90, new TimeSpan[] { TimeSpan.Parse("00:00"), TimeSpan.Parse("23:59:59.999") } }
            }
        };
    }
}

/*
// So sieht das Ergebnis des API-Calls in der Rohfassung aus:

{
    "latitude": 51.22,
    "longitude": 6.7799993,
    "generationtime_ms": 0.08106231689453125,
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
            "2024-05-01T23:00",
            "2024-05-02T00:00",
            "2024-05-02T01:00",
            "2024-05-02T02:00",
            "2024-05-02T03:00",
            "2024-05-02T04:00",
            "2024-05-02T05:00",
            "2024-05-02T06:00",
            "2024-05-02T07:00",
            "2024-05-02T08:00",
            "2024-05-02T09:00",
            "2024-05-02T10:00",
            "2024-05-02T11:00",
            "2024-05-02T12:00",
            "2024-05-02T13:00",
            "2024-05-02T14:00",
            "2024-05-02T15:00",
            "2024-05-02T16:00",
            "2024-05-02T17:00",
            "2024-05-02T18:00",
            "2024-05-02T19:00",
            "2024-05-02T20:00",
            "2024-05-02T21:00",
            "2024-05-02T22:00",
            "2024-05-02T23:00",
            "2024-05-03T00:00",
            "2024-05-03T01:00",
            "2024-05-03T02:00",
            "2024-05-03T03:00",
            "2024-05-03T04:00",
            "2024-05-03T05:00",
            "2024-05-03T06:00",
            "2024-05-03T07:00",
            "2024-05-03T08:00",
            "2024-05-03T09:00",
            "2024-05-03T10:00",
            "2024-05-03T11:00",
            "2024-05-03T12:00",
            "2024-05-03T13:00",
            "2024-05-03T14:00",
            "2024-05-03T15:00",
            "2024-05-03T16:00",
            "2024-05-03T17:00",
            "2024-05-03T18:00",
            "2024-05-03T19:00",
            "2024-05-03T20:00",
            "2024-05-03T21:00",
            "2024-05-03T22:00",
            "2024-05-03T23:00",
            "2024-05-04T00:00",
            "2024-05-04T01:00",
            "2024-05-04T02:00",
            "2024-05-04T03:00",
            "2024-05-04T04:00",
            "2024-05-04T05:00",
            "2024-05-04T06:00",
            "2024-05-04T07:00",
            "2024-05-04T08:00",
            "2024-05-04T09:00",
            "2024-05-04T10:00",
            "2024-05-04T11:00",
            "2024-05-04T12:00",
            "2024-05-04T13:00",
            "2024-05-04T14:00",
            "2024-05-04T15:00",
            "2024-05-04T16:00",
            "2024-05-04T17:00",
            "2024-05-04T18:00",
            "2024-05-04T19:00",
            "2024-05-04T20:00",
            "2024-05-04T21:00",
            "2024-05-04T22:00",
            "2024-05-04T23:00"
        ],
        "temperature_2m": [
            14.7,
            14.3,
            13.4,
            13.3,
            12.3,
            12.6,
            12.7,
            13.4,
            14.8,
            15.1,
            16.2,
            17.2,
            17.6,
            18.0,
            18.0,
            18.4,
            18.3,
            17.5,
            17.0,
            16.6,
            15.8,
            14.7,
            13.7,
            12.9,
            12.2,
            11.5,
            11.0,
            10.3,
            9.9,
            9.7,
            9.5,
            9.6,
            10.5,
            12.2,
            14.1,
            16.0,
            17.5,
            18.5,
            19.5,
            20.4,
            20.4,
            20.3,
            20.3,
            19.4,
            18.5,
            17.5,
            16.5,
            15.8,
            15.4,
            14.7,
            14.3,
            13.9,
            13.4,
            13.2,
            13.0,
            13.0,
            13.4,
            14.5,
            15.4,
            17.1,
            18.6,
            19.8,
            21.2,
            22.3,
            23.0,
            23.5,
            24.0,
            22.9,
            21.7,
            20.4,
            19.3,
            18.3,
            17.6,
            17.0,
            16.3,
            15.0,
            14.2,
            13.6,
            13.3,
            13.3,
            14.1,
            15.5,
            17.5,
            19.3,
            20.8,
            22.2,
            23.4,
            24.3,
            24.9,
            25.2,
            25.2,
            24.9,
            24.2,
            22.9,
            21.3,
            19.8,
            18.5,
            17.2,
            16.2,
            15.4,
            15.0,
            14.7,
            14.5,
            14.6,
            15.1,
            16.1,
            17.5,
            18.9,
            20.3,
            21.8,
            23.1,
            23.9,
            24.5,
            24.8,
            24.7,
            24.2,
            23.4,
            22.0,
            20.2,
            18.8,
            18.2,
            18.0,
            17.9,
            16.6,
            15.8,
            15.3,
            15.1,
            15.2,
            16.0,
            17.8,
            20.2,
            22.3,
            23.5,
            24.3,
            24.8,
            25.1,
            25.0,
            24.7,
            24.2,
            23.6,
            22.7,
            21.5,
            20.2,
            19.0,
            17.9,
            16.8,
            15.8,
            14.6,
            13.5,
            12.8,
            12.5,
            12.6,
            13.3,
            14.7,
            16.7,
            18.8,
            20.9,
            23.1,
            24.6,
            24.9,
            24.6,
            24.2,
            23.8,
            23.4,
            22.6,
            21.2,
            19.5,
            18.0
        ],
        "relative_humidity_2m": [
            62,
            61,
            72,
            70,
            77,
            72,
            69,
            66,
            65,
            65,
            60,
            52,
            47,
            45,
            45,
            41,
            39,
            43,
            43,
            44,
            46,
            52,
            60,
            60,
            60,
            65,
            67,
            71,
            76,
            80,
            80,
            80,
            76,
            71,
            65,
            58,
            51,
            44,
            36,
            36,
            36,
            39,
            38,
            44,
            50,
            57,
            63,
            66,
            69,
            73,
            76,
            79,
            82,
            84,
            85,
            85,
            83,
            77,
            72,
            65,
            57,
            52,
            51,
            50,
            50,
            47,
            46,
            57,
            66,
            72,
            77,
            80,
            82,
            84,
            88,
            92,
            95,
            97,
            99,
            100,
            96,
            89,
            80,
            72,
            66,
            62,
            58,
            55,
            53,
            52,
            53,
            54,
            58,
            64,
            73,
            80,
            86,
            91,
            95,
            97,
            97,
            97,
            97,
            96,
            94,
            89,
            82,
            75,
            67,
            58,
            52,
            49,
            48,
            48,
            49,
            51,
            55,
            64,
            75,
            80,
            73,
            59,
            49,
            50,
            52,
            54,
            58,
            62,
            63,
            58,
            51,
            44,
            40,
            38,
            36,
            33,
            31,
            30,
            31,
            35,
            39,
            45,
            52,
            59,
            67,
            76,
            83,
            86,
            87,
            87,
            87,
            86,
            83,
            77,
            69,
            61,
            54,
            47,
            42,
            40,
            41,
            42,
            43,
            45,
            48,
            52,
            56,
            59
        ],
        "weather_code": [
            3,
            3,
            3,
            3,
            3,
            3,
            2,
            3,
            80,
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
            0,
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
            80,
            61,
            80,
            61,
            61,
            61,
            3,
            61,
            3,
            3,
            2,
            2,
            2,
            2,
            3,
            3,
            2,
            2,
            1,
            1,
            3,
            2,
            3,
            3,
            1,
            3,
            45,
            45,
            45,
            3,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            0,
            0,
            0,
            1,
            1,
            1,
            45,
            45,
            45,
            45,
            45,
            45,
            3,
            3,
            3,
            2,
            2,
            2,
            1,
            1,
            1,
            2,
            2,
            2,
            2,
            2,
            2,
            0,
            0,
            0,
            1,
            1,
            1,
            3,
            3,
            3,
            0,
            0,
            0,
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
            1,
            1,
            1,
            2,
            2,
            2,
            1,
            1,
            1,
            0,
            0,
            0,
            1,
            1,
            1,
            2,
            2,
            2,
            2,
            2,
            2,
            0,
            0,
            0
        ]
    },
    "daily_units": {
        "time": "iso8601",
        "sunrise": "iso8601",
        "sunset": "iso8601"
    },
    "daily": {
        "time": [
            "2024-04-28",
            "2024-04-29",
            "2024-04-30",
            "2024-05-01",
            "2024-05-02",
            "2024-05-03",
            "2024-05-04"
        ],
        "sunrise": [
            "2024-04-28T06:10",
            "2024-04-29T06:08",
            "2024-04-30T06:06",
            "2024-05-01T06:04",
            "2024-05-02T06:02",
            "2024-05-03T06:00",
            "2024-05-04T05:58"
        ],
        "sunset": [
            "2024-04-28T20:50",
            "2024-04-29T20:52",
            "2024-04-30T20:53",
            "2024-05-01T20:55",
            "2024-05-02T20:57",
            "2024-05-03T20:58",
            "2024-05-04T21:00"
        ]
    }
}

*/
