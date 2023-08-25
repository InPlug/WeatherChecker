using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace WeatherChecker
{
    /// <summary>
    /// Class to receive the return from the api-call to http://ip-api.com/json/.
    /// 25.08.2023 Erik Nagel: created.
    /// </summary>
    [DataContract]
    public class GeoLocation_ReturnObject
    {
        // Root GeoLocation_ReturnObject = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        /// <summary>
        /// Success or fail
        /// </summary>
        [DataMember]
        public string? status { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        [DataMember]
        public string? country { get; set; }

        /// <summary>
        /// wo-letter country code ISO 3166-1 alpha-2
        /// </summary>
        [DataMember]
        public string? countryCode { get; set; }

        /// <summary>
        /// Region/state short code (FIPS or ISO)
        /// </summary>
        [DataMember]
        public string? region { get; set; }

        /// <summary>
        /// Region/state
        /// </summary>
        [DataMember]
        public string? regionName { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [DataMember]
        public string? city { get; set; }

        /// <summary>
        /// Zip code
        /// </summary>
        [DataMember]
        public string? zip { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [DataMember]
        public double? lat { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [DataMember]
        public double? lon { get; set; }

        /// <summary>
        /// Timezone (tz)
        /// </summary>
        [DataMember]
        public string? timezone { get; set; }

        /// <summary>
        /// ISP name
        /// </summary>
        [DataMember]
        public string? isp { get; set; }

        /// <summary>
        /// Organization name
        /// </summary>
        [DataMember]
        public string? org { get; set; }

        /// <summary>
        /// AS number and organization, separated by space (RIR). Empty for IP blocks not being announced in BGP tables.
        /// </summary>
        [DataMember]
        [JsonProperty("as")]
        public string? company { get; set; }

        /// <summary>
        /// P used for the query
        /// </summary>
        [DataMember]
        public string? query { get; set; }

        /// <summary>
        /// Standard Konstruktor.
        /// </summary>
        public GeoLocation_ReturnObject() { }

        /// <summary>
        /// Deserialisierungs-Konstruktor.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Übertragungs-Kontext.</param>
        protected GeoLocation_ReturnObject(SerializationInfo info, StreamingContext context)
        {
            this.status = info.GetString("status");
            this.country = info.GetString("country");
            this.countryCode = info.GetString("countryCode");
            this.region = info.GetString("region");
            this.regionName = info.GetString("regionName");
            this.city = info.GetString("city");
            this.zip = info.GetString("zip");
            this.lat = (double?)info.GetValue("lat", typeof(double));
            this.lon = (double?)info.GetValue("lon", typeof(double));
            this.timezone = info.GetString("timezone");
            this.isp = info.GetString("isp");
            this.org = info.GetString("org");
            this.company = info.GetString("company");
            this.query = info.GetString("query");
        }

        /// <summary>
        /// Serialisierungs-Hilfsroutine: holt die Objekt-Properties in den Property-Container.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Serialisierungs-Kontext.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("status", this.status);
            info.AddValue("country", this.country);
            info.AddValue("countryCode", this.countryCode);
            info.AddValue("region", this.region);
            info.AddValue("regionName", this.regionName);
            info.AddValue("city", this.city);
            info.AddValue("zip", this.zip);
            info.AddValue("lat", this.lat);
            info.AddValue("lon", this.lon);
            info.AddValue("timezone", this.timezone);
            info.AddValue("isp", this.isp);
            info.AddValue("org", this.org);
            info.AddValue("company", this.company);
            info.AddValue("query", this.query);
        }

        /// <summary>
        /// Überschriebene ToString()-Methode - stellt alle öffentlichen Properties
        /// als einen aufbereiteten String zur Verfügung.
        /// </summary>
        /// <returns>Alle öffentlichen Properties als ein String aufbereitet.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("status: " + this.status);
            string delimiter = Environment.NewLine;
            stringBuilder.Append(delimiter + "country: " + this.country);
            stringBuilder.Append(delimiter + "countryCode: " + this.countryCode);
            stringBuilder.Append(delimiter + "region: " + this.region);
            stringBuilder.Append(delimiter + "regionName: " + this.regionName);
            stringBuilder.Append(delimiter + "city: " + this.city);
            stringBuilder.Append(delimiter + "zip: " + this.zip);
            stringBuilder.Append(delimiter + "lat: " + this.lat);
            stringBuilder.Append(delimiter + "lon: " + this.lon);
            stringBuilder.Append(delimiter + "timezone: " + this.timezone);
            stringBuilder.Append(delimiter + "isp: " + this.isp);
            stringBuilder.Append(delimiter + "org: " + this.org);
            stringBuilder.Append(delimiter + "company: " + this.company);
            stringBuilder.Append(delimiter + "query: " + this.query);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Vergleicht dieses Objekt mit einem übergebenen Objekt nach Inhalt.
        /// </summary>
        /// <param name="obj">Das zu vergleichende GeoLocation_ReturnObject.</param>
        /// <returns>True, wenn das übergebene GeoLocation_ReturnObject inhaltlich gleich diesem GeoLocation_ReturnObject ist.</returns>
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
            return this.ToString().Equals(obj.ToString());
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
}
