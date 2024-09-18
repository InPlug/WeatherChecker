using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
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
        [DataMember(Name = "time")]
        public string? Time { get; set; }

        /// <summary>
        /// The unit of measurement for the 2-meters temperature data (e.g., "celsius").
        /// </summary>
        [DataMember(Name = "temperature_2m")]
        public string? Temperature2m { get; set; }

        /// <summary>
        /// The unit of measurement for the relative humidity at 2 meters data (e.g., "%").
        /// </summary>
        [DataMember(Name = "relative_humidity_2m")]
        public string? RelativeHumidity2m { get; set; }

        /// <summary>
        /// The unit of measurement for the apparent temperature data (e.g., "celsius").
        /// </summary>
        [DataMember(Name = "apparent_temperature")]
        public string? ApparentTemperature { get; set; }

        /// <summary>
        /// The unit of measurement for the precipitation probability data (e.g., "%").
        /// </summary>
        [DataMember(Name = "precipitation_probability")]
        public string? PrecipitationProbability { get; set; }

        /// <summary>
        /// The unit of measurement for the rain data (e.g., "mm").
        /// </summary>
        [DataMember(Name = "rain")]
        public string? Rain { get; set; }

        /// <summary>
        /// The unit of measurement for the showers data (e.g., "mm").
        /// </summary>
        [DataMember(Name = "showers")]
        public string? Showers { get; set; }

        /// <summary>
        /// The unit of measurement for the snowfall data (e.g., "cm").
        /// </summary>
        [DataMember(Name = "snowfall")]
        public string? Snowfall { get; set; }

        /// <summary>
        /// The unit of measurement for the snow depth data (e.g., "m").
        /// </summary>
        [DataMember(Name = "snow_depth")]
        public string? SnowDepth { get; set; }

        /// <summary>
        /// The unit of measurement for the weather code data (e.g., "wmo code").
        /// </summary>
        [DataMember(Name = "weather_code")]
        public string? WeatherCode { get; set; }

        /// <summary>
        /// The unit of measurement for the surface pressure data (e.g., "hPa").
        /// </summary>
        [DataMember(Name = "surface_pressure")]
        public string? SurfacePressure { get; set; }

        /// <summary>
        /// The unit of measurement for the cloud cover data (e.g., "%").
        /// </summary>
        [DataMember(Name = "cloud_cover")]
        public string? CloudCover { get; set; }

        /// <summary>
        /// The unit of measurement for the 10-meters wind speed data (e.g., "km/h").
        /// </summary>
        [DataMember(Name = "wind_speed_10m")]
        public string? WindSpeed10m { get; set; }

        /// <summary>
        /// The unit of measurement for the 10-meters wind direction data (e.g., "%").
        /// </summary>
        [DataMember(Name = "wind_direction_10m")]
        public string? WindDirection10m { get; set; }

        /// <summary>
        /// The unit of measurement for the 10-meters wind gusts data (e.g., "km/h").
        /// </summary>
        [DataMember(Name = "wind_gusts_10m")]
        public string? WindGusts10m { get; set; }

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
        [DataMember(Name = "time")]
        public List<string>? Time { get; set; }

        /// <summary>
        /// List of 2-meter temperature values in the specified unit (from HourlyUnits.Temperature2m).
        /// </summary>
        [DataMember(Name = "temperature_2m")]
        public List<double>? Temperature2m { get; set; }

        /// <summary>
        /// List of relative humidity values at 2 meters (from HourlyUnits.RelativeHumidity2m).
        /// </summary>
        [DataMember(Name = "relative_humidity_2m")]
        public List<int>? RelativeHumidity2m { get; set; }

        /// <summary>
        /// List of apparent temperature values (from HourlyUnits.ApparentTemperature).
        /// </summary>
        [DataMember(Name = "apparent_temperature")]
        public List<double>? ApparentTemperature { get; set; }

        /// <summary>
        /// List of precipitation probability values (from HourlyUnits.PrecipitationProbability).
        /// </summary>
        [DataMember(Name = "precipitation_probability")]
        public List<int>? PrecipitationProbability { get; set; }

        /// <summary>
        /// List of rain values (from HourlyUnits.Rain).
        /// </summary>
        [DataMember(Name = "rain")]
        public List<double>? Rain { get; set; }

        /// <summary>
        /// List of showers values (from HourlyUnits.Showers).
        /// </summary>
        [DataMember(Name = "showers")]
        public List<double>? Showers { get; set; }

        /// <summary>
        /// List of snowfall values (from HourlyUnits.Snowfall).
        /// </summary>
        [DataMember(Name = "snowfall")]
        public List<double>? Snowfall { get; set; }

        /// <summary>
        /// List of weather codes for each data point.
        /// </summary>
        [DataMember(Name = "weather_code")]
        public List<int>? WeatherCode { get; set; }

        /// <summary>
        /// List of surface pressure data (from HourlyUnits.SurfacePressure).
        /// </summary>
        [DataMember(Name = "surface_pressure")]
        public List<double>? SurfacePressure { get; set; }

        /// <summary>
        /// List of cloud cover data (from HourlyUnits.CloudCover).
        /// </summary>
        [DataMember(Name = "cloud_cover")]
        public List<int>? CloudCover { get; set; }

        /// <summary>
        /// List of 10-meters wind speed data (from HourlyUnits.SurfacePressure).
        /// </summary>
        [DataMember(Name = "wind_speed_10m")]
        public List<double>? WindSpeed10m { get; set; }

        /// <summary>
        /// The unit of measurement for the 10-meters wind direction data (e.g., "%").
        /// </summary>
        [DataMember(Name = "wind_direction_10m")]
        public List<int>? WindDirection10m { get; set; }

        /// <summary>
        /// The unit of measurement for the 10-meters wind gusts data (e.g., "km/h").
        /// </summary>
        [DataMember(Name = "wind_gusts_10m")]
        public List<double>? WindGusts10m { get; set; }

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
            ApparentTemperature = (List<double>?)info.GetValue("ApparentTemperature", typeof(List<double>));
            PrecipitationProbability = (List<int>?)info.GetValue("PrecipitationProbability", typeof(List<int>));
            Rain = (List<double>?)info.GetValue("Rain", typeof(List<double>));
            Showers = (List<double>?)info.GetValue("Showers", typeof(List<double>));
            Snowfall = (List<double>?)info.GetValue("Snowfall", typeof(List<double>));
            WeatherCode = (List<int>?)info.GetValue("WeatherCode", typeof(List<int>));
            SurfacePressure = (List<double>?)info.GetValue("SurfacePressure", typeof(List<double>));
            CloudCover = (List<int>?)info.GetValue("CloudCover", typeof(List<int>));
            WindSpeed10m = (List<double>?)info.GetValue("WindSpeed10m", typeof(List<double>));
            WindDirection10m = (List<int>?)info.GetValue("WindDirection10m", typeof(List<int>));
            WindGusts10m = (List<double>?)info.GetValue("WindGusts10m", typeof(List<double>));
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
            info.AddValue("ApparentTemperature", ApparentTemperature);
            info.AddValue("PrecipitationProbability", PrecipitationProbability);
            info.AddValue("Rain", Rain);
            info.AddValue("Showers", Showers);
            info.AddValue("Snowfall", Snowfall);
            info.AddValue("WeatherCode", WeatherCode);
            info.AddValue("SurfacePressure", SurfacePressure);
            info.AddValue("CloudCover", CloudCover);
            info.AddValue("WindSpeed10m", WindSpeed10m);
            info.AddValue("WindDirection10m", WindDirection10m);
            info.AddValue("WindGusts10m", WindGusts10m);
        }

        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>This object.ToString().</returns>
        public override string ToString()
        {
            return $"Time: {string.Join(", ", Time ?? new List<string>())}"
                 + $", Temperature2m: {string.Join(", ", Temperature2m ?? new List<double>())}"
                 + $", RelativeHumidity2m: {string.Join(", ", RelativeHumidity2m ?? new List<int>())}"
                 + $", ApparentTemperature: {string.Join(", ", ApparentTemperature ?? new List<double>())}"
                 + $", PrecipitationProbability: {string.Join(", ", PrecipitationProbability ?? new List<int>())}"
                 + $", Rain: {string.Join(", ", Rain ?? new List<double>())}"
                 + $", Showers: {string.Join(", ", Showers ?? new List<double>())}"
                 + $", Snowfall: {string.Join(", ", Snowfall ?? new List<double>())}"
                 + $", WeatherCode: {string.Join(", ", WeatherCode ?? new List<int>())}"
                 + $", SurfacePressure: {string.Join(", ", SurfacePressure ?? new List<double>())}"
                 + $", CloudCover: {string.Join(", ", CloudCover ?? new List<int>())}"
                 + $", WindSpeed10m: {string.Join(", ", WindSpeed10m ?? new List<double>())}"
                 + $", WindDirection10m: {string.Join(", ", WindDirection10m ?? new List<int>())}"
                 + $", WindGusts10m: {string.Join(", ", WindGusts10m ?? new List<double>())}";
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
        /// The unit of measurement for the 2-meters maximum temperature data (e.g., "celsius").
        /// </summary>
        [DataMember(Name = "temperature_2m_max")]
        public string? Temperature2mMax { get; set; }

        /// <summary>
        /// The unit of measurement for the maximum apparent temperature data (e.g., "celsius").
        /// </summary>
        [DataMember(Name = "apparent_temperature_max")]
        public string? ApparentTemperatureMax { get; set; }

        /// <summary>
        /// The unit of time used for sunrise and sunset times (e.g., "ISO8601").
        /// </summary>
        [DataMember(Name = "sunrise")]
        public string? Sunrise { get; set; }

        /// <summary>
        /// The unit of time used for sunrise and sunset times (e.g., "ISO8601").
        /// </summary>
        [DataMember(Name = "sunset")]
        public string? Sunset { get; set; }

        /// <summary>
        /// The unit of time used for sunshine duration (e.g., "s").
        /// </summary>
        [DataMember(Name = "sunshine_duration")]
        public string? SunshineDuration { get; set; }

        /// <summary>
        /// The unit of measurement for the rain sum data (e.g., "mm").
        /// </summary>
        [DataMember(Name = "rain_sum")]
        public string? RainSum { get; set; }

        /// <summary>
        /// The unit of measurement for the showers sum data (e.g., "mm").
        /// </summary>
        [DataMember(Name = "showers_sum")]
        public string? ShowersSum { get; set; }

        /// <summary>
        /// The unit of measurement for the snowfall sum data (e.g., "cm").
        /// </summary>
        [DataMember(Name = "snowfall_sum")]
        public string? SnowfallSum { get; set; }

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
        [DataMember(Name = "time")]
        public List<string>? Time { get; set; }

        /// <summary>
        /// List of 2-meter temperature maximum values in the specified unit (from DailyUnits.Temperature2mMax).
        /// </summary>
        [DataMember(Name = "temperature_2m_max")]
        public List<double>? Temperature2mMax { get; set; }

        /// <summary>
        /// List of apparent temperature maximum values (from DailyUnits.ApparentTemperatureMax).
        /// </summary>
        [DataMember(Name = "apparent_temperature_max")]
        public List<double>? ApparentTemperatureMax { get; set; }

        /// <summary>
        /// List of sunrise times for each day (in the unit specified by DailyUnits.Sunrise).
        /// </summary>
        [DataMember(Name = "sunrise")]
        public List<string>? Sunrise { get; set; }

        /// <summary>
        /// List of sunset times for each day (in the unit specified by DailyUnits.Sunset).
        /// </summary>
        [DataMember(Name = "sunset")]
        public List<string>? Sunset { get; set; }

        /// <summary>
        /// List of sunshine duration times for each day (in the unit specified by DailyUnits.SunshineDuration).
        /// </summary>
        [DataMember(Name = "sunshine_duration")]
        public List<string>? SunshineDuration { get; set; }

        /// <summary>
        /// List of rain sum values (from DailyUnits.RainSum).
        /// </summary>
        [DataMember(Name = "rain_sum")]
        public List<double>? RainSum { get; set; }

        /// <summary>
        /// List of showers sum values (from DailyUnits.ShowersSum).
        /// </summary>
        [DataMember(Name = "showers_sum")]
        public List<double>? ShowersSum { get; set; }

        /// <summary>
        /// List of snowfall sum values (from DailyUnits.SnowfallSum).
        /// </summary>
        [DataMember(Name = "snowfall_sum")]
        public List<double>? SnowfallSum { get; set; }

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
            Temperature2mMax = (List<double>?)info.GetValue("Temperature2mMax", typeof(List<double>));
            ApparentTemperatureMax = (List<double>?)info.GetValue("ApparentTemperatureMax", typeof(List<double>));
            Sunrise = (List<string>?)info.GetValue("Sunrise", typeof(List<string>));
            Sunset = (List<string>?)info.GetValue("Sunset", typeof(List<string>));
            SunshineDuration = (List<string>?)info.GetValue("SunshineDuration", typeof(List<string>));
            RainSum = (List<double>?)info.GetValue("RainSum", typeof(List<double>));
            ShowersSum = (List<double>?)info.GetValue("ShowersSum", typeof(List<double>));
            SnowfallSum = (List<double>?)info.GetValue("SnowfallSum", typeof(List<double>));
        }

        /// <summary>
        /// Serialization method.
        /// </summary>
        /// <param name="info">Property container.</param>
        /// <param name="context">Transscription context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Time", Time);
            info.AddValue("Temperature2mMax", Temperature2mMax);
            info.AddValue("ApparentTemperatureMax", ApparentTemperatureMax);
            info.AddValue("Sunrise", Sunrise);
            info.AddValue("Sunset", Sunset);
            info.AddValue("SunshineDuration", SunshineDuration);
            info.AddValue("RainSum", RainSum);
            info.AddValue("ShowersSum", ShowersSum);
            info.AddValue("SnowfallSum", SnowfallSum);
        }

        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>This object.ToString().</returns>
        public override string ToString()
        {
            return $"Time: {string.Join(", ", Time ?? new List<string>())}"
                 + $", Temperature2mMax: {string.Join(", ", Temperature2mMax ?? new List<double>())}"
                 + $", ApparentTemperatureMax: {string.Join(", ", ApparentTemperatureMax ?? new List<double>())}"
                 + $", Sunrise: {string.Join(", ", Sunrise ?? new List<string>())}"
                 + $", Sunset: {string.Join(", ", Sunset ?? new List<string>())}"
                 + $", SunshineDuration: {string.Join(", ", SunshineDuration ?? new List<string>())}"
                 + $", RainSum: {string.Join(", ", RainSum ?? new List<double>())}"
                 + $", ShowersSum: {string.Join(", ", ShowersSum ?? new List<double>())}"
                 + $", SnowfallSum: {string.Join(", ", SnowfallSum ?? new List<double>())}";
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
        [DataMember(Name = "latitude")]
        public double? Latitude { get; set; }

        /// <summary>
        /// Geographic longitude of the requested location.
        /// </summary>
        [DataMember(Name = "longitude")]
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
        [DataMember(Name = "elevation")]
        public double? Elevation { get; set; }

        /// <summary>
        /// Hourly weather data.
        /// </summary>
        [DataMember(Name = "hourly_units")]
        public HourlyUnits? HourlyUnits { get; set; }

        /// <summary>
        /// Hourly weather data points.
        /// </summary>
        [DataMember(Name = "hourly")]
        public Hourly? Hourly { get; set; }

        /// <summary>
        /// Daily data units.
        /// </summary>
        [DataMember(Name = "daily_units")]
        public DailyUnits? DailyUnits { get; set; }

        /// <summary>
        /// Daily weather data points.
        /// </summary>
        [DataMember(Name = "daily")]
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
            return $"Latitude: {Latitude}"
                 + $", Longitude: {Longitude}"
                 + $", GenerationTimeMs: {GenerationTimeMs}"
                 + $", UTCOffsetSeconds: {UtcOffsetSeconds}"
                 + $", Timezone: {Timezone}"
                 + $", TimezoneAbbreviation: {TimezoneAbbreviation}"
                 + $", Elevation: {Elevation}"
                 + $", HourlyUnits: {HourlyUnits}"
                 + $", Hourly: {Hourly}"
                 + $", DailyUnits: {DailyUnits}"
                 + $", Daily: {Daily}";
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

        internal static WeatherChecker_ReturnObject? ConvertToWeatherChecker_ReturnObject(
            WeatherChecker_OpenMeteo_ReturnObject? deserializedWeather, GeoLocation_ReturnObject? geoLocation_ReturnObject)
        {
            WeatherChecker_ReturnObject? weatherChecker_ReturnObject = new WeatherChecker_ReturnObject();
            if (geoLocation_ReturnObject == null) return weatherChecker_ReturnObject;
            weatherChecker_ReturnObject.Location = geoLocation_ReturnObject;
            if (deserializedWeather == null || deserializedWeather.Hourly == null || deserializedWeather.Daily == null)
                return weatherChecker_ReturnObject;
            weatherChecker_ReturnObject.Location.Elevation = deserializedWeather.Elevation;
            // WeatherChecker_OpenMeteo_ReturnObject enthält pro Stunde einen Datenpunkt, WeatherChecker_ReturnObject
            // nur pro alle drei Stunden.
            // Deshalb wird in den nächsten beiden Loops nur jeder dritte Datenpunkt des WeatherChecker_OpenMeteo_ReturnObjects
            // referenziert.
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
                string timePoint = weatherChecker_ReturnObject.Dataseries[i].Timepoint
                    ?? throw new ArgumentNullException("Timepoint");
                int dailyIndex = FindDailyIndex(deserializedWeather, timePoint)
                    ?? throw new ArgumentNullException("DailyIndex");
                string sunriseString = deserializedWeather.Daily.Sunrise?[dailyIndex] ?? throw new ArgumentNullException("Sunrise");
                string sunsetString = deserializedWeather.Daily.Sunset?[dailyIndex] ?? throw new ArgumentNullException("Sunset");
                // ConversionHelpers.FetchSunriseAndSunset(deserializedWeather, timePoint, out string sunriseString,
                // out string sunsetString);
                weatherChecker_ReturnObject.Dataseries[i].Sunrise = sunriseString;
                weatherChecker_ReturnObject.Dataseries[i].Sunset = sunsetString;
                weatherChecker_ReturnObject.Dataseries[i].SunshineDuration = deserializedWeather.Daily.SunshineDuration?[dailyIndex]
                    + " s" ?? throw new ArgumentNullException("SunshineDuration");
                weatherChecker_ReturnObject.Dataseries[i].RainSum = deserializedWeather.Daily.RainSum?[dailyIndex].ToString()
                    + " mm" ?? throw new ArgumentNullException("RainSum");
                weatherChecker_ReturnObject.Dataseries[i].ShowersSum = deserializedWeather.Daily.RainSum?[dailyIndex].ToString()
                    + " mm" ?? throw new ArgumentNullException("ShowersSum");
                weatherChecker_ReturnObject.Dataseries[i].SnowfallSum = deserializedWeather.Daily.SnowfallSum?[dailyIndex].ToString()
                    + " cm" ?? throw new ArgumentNullException("SnowfallSum");
                weatherChecker_ReturnObject.Dataseries[i].Temperature
                    = deserializedWeather.Hourly.Temperature2m?[j].ToString() + " °C";
                weatherChecker_ReturnObject.Dataseries[i].Humidity
                    = deserializedWeather.Hourly.RelativeHumidity2m?[j].ToString() + " %";
                weatherChecker_ReturnObject.Dataseries[i].ApparentTemperature
                    = deserializedWeather.Hourly.ApparentTemperature?[j].ToString() + " °C";
                weatherChecker_ReturnObject.Dataseries[i].PrecipitationProbability
                    = deserializedWeather.Hourly.PrecipitationProbability?[j].ToString() + " %";
                weatherChecker_ReturnObject.Dataseries[i].Rain = deserializedWeather.Hourly.Rain?[j].ToString() + " mm";
                weatherChecker_ReturnObject.Dataseries[i].Showers = deserializedWeather.Hourly.Showers?[j].ToString() + " mm";
                weatherChecker_ReturnObject.Dataseries[i].Snowfall = deserializedWeather.Hourly.Snowfall?[j].ToString() + " cm";
                weatherChecker_ReturnObject.Dataseries[i].Weather
                    = ConversionHelpers.WMONametoSimplifiedWeatherNameWithDayOrNight(deserializedWeather.Hourly.WeatherCode?[j]
                    ?? 0, timePoint, sunriseString, sunsetString);
                weatherChecker_ReturnObject.Dataseries[i].SurfacePressure
                    = deserializedWeather.Hourly.SurfacePressure?[j].ToString() + " hPa";
                weatherChecker_ReturnObject.Dataseries[i].Cloudcover
                    = deserializedWeather.Hourly.CloudCover?[j].ToString() + " %";
                WeatherChecker_ReturnObject.ForecastDataPoint.Wind wind10m
                    = new WeatherChecker_ReturnObject.ForecastDataPoint.Wind();
                wind10m.Speed = deserializedWeather.Hourly.WindSpeed10m?[j].ToString() + " m/s";
                wind10m.Direction = ConversionHelpers.WindDirectionToCompassDirection(
                    deserializedWeather.Hourly.WindDirection10m?[j] ?? 0);
                wind10m.GustsSpeed = deserializedWeather.Hourly.WindGusts10m?[j].ToString() + " m/s";
                weatherChecker_ReturnObject.Dataseries[i].Wind10m = wind10m;
            }
            return weatherChecker_ReturnObject;
        }

        private static int? FindDailyIndex(WeatherChecker_OpenMeteo_ReturnObject deserializedWeather, string timePoint)
        {
            return deserializedWeather.Daily?.Time?.FindIndex(x => x == timePoint.Substring(0, 10));
        }
    }

    static class ConversionHelpers
    {
        internal static string WMONametoSimplifiedWeatherNameWithDayOrNight(int wmoCode, string dateTimeString, string sunriseString, string sunsetString /* "2024-04-29T09:00" */)
        {
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

        internal static string WindDirectionToCompassDirection(int windDirection)
        {
            int index = (int)(windDirection / 22.5 + 0.5);
            return WindDirectionNames[index];
        }

        /// <summary>
        /// This dictionary maps WMO weather codes to simplified weather names.
        /// </summary>
        internal static readonly string[] WindDirectionNames
            = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };

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
        internal static string WMONametoSimplifiedWeatherNameWithDayOrNight(int wmoCode, int latitude, string dateTimeString) // "2024-04-29T09:00"
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
        */

        /*
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
        */

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

        /*
        internal static bool IsDayLight(int wmoCode, int latitude, string dateTimeString) // "2024-04-29T09:00"
        {
            DateTime dateTime = DateTime.Parse(dateTimeString);
            int quarter = DateTimeToQuarter(dateTime);
            TimeSpan daylightFrom = _quarterLatitudeTime[quarter][latitude][0];
            TimeSpan daylightTo = _quarterLatitudeTime[quarter][latitude][1];
            TimeSpan timeNow = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
            return (daylightFrom < daylightTo && timeNow >= daylightFrom && timeNow < daylightTo);
        }
        */

        /*
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
        */
    }
}

/*
// So sieht das Ergebnis des API-Calls in der Rohfassung aus:

{
	"latitude": 51.268,
	"longitude": 6.8149996,
	"generationtime_ms": 0.10693073272705078,
	"utc_offset_seconds": 7200,
	"timezone": "Europe/Berlin",
	"timezone_abbreviation": "CEST",
	"elevation": 39.0,
	"hourly_units": {
		"time": "iso8601",
		"temperature_2m": "°C",
		"relative_humidity_2m": "%",
		"apparent_temperature": "°C",
		"precipitation_probability": "%",
		"precipitation": "mm",
		"rain": "mm",
		"showers": "mm",
		"snowfall": "cm",
		"snow_depth": "m",
		"weather_code": "wmo code",
		"surface_pressure": "hPa",
		"wind_speed_10m": "km/h",
		"wind_direction_10m": "%",
		"wind_gusts_10m": "km/h"
	},
	"hourly": {
		"time": [
			"2024-09-09T00:00",
			"2024-09-09T01:00",
			"2024-09-09T02:00",
			"2024-09-09T03:00",
			"2024-09-09T04:00",
			"2024-09-09T05:00",
			"2024-09-09T06:00",
			"2024-09-09T07:00",
			"2024-09-09T08:00",
			"2024-09-09T09:00",
			"2024-09-09T10:00",
			"2024-09-09T11:00",
			"2024-09-09T12:00",
			"2024-09-09T13:00",
			"2024-09-09T14:00",
			"2024-09-09T15:00",
			"2024-09-09T16:00",
			"2024-09-09T17:00",
			"2024-09-09T18:00",
			"2024-09-09T19:00",
			"2024-09-09T20:00",
			"2024-09-09T21:00",
			"2024-09-09T22:00",
			"2024-09-09T23:00"
		],
		"temperature_2m": [
			18.5,
			17.8,
			17.9,
			16.3,
			16.0,
			16.2,
			15.7,
			16.0,
			15.8,
			16.2,
			17.9,
			17.2,
			16.5,
			16.7,
			18.0,
			19.2,
			19.5,
			19.6,
			20.5,
			19.2,
			18.0,
			17.2,
			16.8,
			16.8
		],
		"relative_humidity_2m": [
			62,
			65,
			66,
			71,
			76,
			72,
			75,
			75,
			85,
			85,
			79,
			81,
			85,
			86,
			84,
			75,
			73,
			71,
			69,
			73,
			81,
			92,
			92,
			89
		],
		"apparent_temperature": [
			17.5,
			17.3,
			17.3,
			15.2,
			14.9,
			15.5,
			14.9,
			15.7,
			15.8,
			15.8,
			18.0,
			17.1,
			17.0,
			17.0,
			18.5,
			18.7,
			19.4,
			19.7,
			20.9,
			18.8,
			17.5,
			17.8,
			16.6,
			16.3
		],
		"precipitation_probability": [
			0,
			0,
			0,
			0,
			0,
			0,
			13,
			32,
			49,
			61,
			71,
			78,
			80,
			78,
			78,
			81,
			84,
			86,
			85,
			82,
			80,
			80,
			79,
			76
		],
		"precipitation": [
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.40,
			0.00,
			0.00,
			0.00,
			0.10,
			0.50,
			0.10,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			2.20,
			0.40,
			0.00
		],
		"rain": [
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00
		],
		"showers": [
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.40,
			0.00,
			0.00,
			0.00,
			0.10,
			0.50,
			0.10,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			2.20,
			0.40,
			0.00
		],
		"snowfall": [
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00
		],
		"snow_depth": [
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00
		],
		"weather_code": [
			0,
			0,
			3,
			0,
			0,
			3,
			3,
			51,
			3,
			0,
			0,
			51,
			53,
			51,
			3,
			3,
			3,
			3,
			0,
			3,
			3,
			61,
			51,
			3
		],
		"surface_pressure": [
			1001.8,
			1001.3,
			1000.2,
			1000.1,
			999.6,
			999.2,
			999.0,
			998.6,
			998.8,
			998.9,
			998.6,
			998.6,
			998.9,
			999.1,
			999.9,
			1000.0,
			1000.1,
			1000.3,
			1000.7,
			1001.0,
			1001.9,
			1002.5,
			1003.2,
			1004.0
		],
		"wind_speed_10m": [
			9.4,
			6.1,
			7.6,
			9.7,
			11.2,
			7.2,
			7.9,
			5.4,
			7.6,
			10.4,
			8.6,
			9.4,
			5.8,
			7.6,
			8.6,
			13.7,
			11.2,
			8.6,
			7.6,
			12.2,
			14.0,
			10.1,
			14.0,
			14.8
		],
		"wind_direction_10m": [
			184,
			176,
			182,
			146,
			140,
			150,
			169,
			145,
			125,
			140,
			152,
			204,
			223,
			277,
			290,
			273,
			292,
			287,
			255,
			291,
			274,
			260,
			276,
			307
		],
		"wind_gusts_10m": [
			15.8,
			11.9,
			14.4,
			17.3,
			19.1,
			14.8,
			17.3,
			12.6,
			15.8,
			20.9,
			21.6,
			19.8,
			18.0,
			15.5,
			18.7,
			27.0,
			27.4,
			23.4,
			17.3,
			35.6,
			33.5,
			26.3,
			29.9,
			27.4
		]
	},
	"daily_units": {
		"time": "iso8601",
		"temperature_2m_max": "°C",
		"apparent_temperature_max": "°C",
		"sunrise": "iso8601",
		"sunset": "iso8601",
		"sunshine_duration": "s",
		"rain_sum": "mm",
		"showers_sum": "mm",
		"snowfall_sum": "cm"
	},
	"daily": {
		"time": [
			"2024-09-09"
		],
		"temperature_2m_max": [
			20.5
		],
		"apparent_temperature_max": [
			20.9
		],
		"sunrise": [
			"2024-09-09T06:59"
		],
		"sunset": [
			"2024-09-09T20:00"
		],
		"sunshine_duration": [
			17152.51
		],
		"rain_sum": [
			0.00
		],
		"showers_sum": [
			3.70
		],
		"snowfall_sum": [
			0.00
		]
	}
}
*/
