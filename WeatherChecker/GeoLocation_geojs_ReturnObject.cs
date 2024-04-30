using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace WeatherChecker
{
    /// <summary>
    /// Class to receive the return from the api-call to https://www.geojs.io/.
    /// Created by Erik Nagel and Bard (Gemini) on 04.03.2024.
    /// 
    /// 04.03.2024 Erik Nagel: erstellt
    /// </summary>
    /// <remarks>
    /// This class corresponds to the following JSON structure:
    /// ```json
    /// {
    ///   "country_code": "DE",
    ///   "country_code3": "DEU",
    ///   "continent_code": "EU",
    ///   "region": "North Rhine-Westphalia",
    ///   "latitude": "51.2402",
    ///   "longitude": "6.7785",
    ///   "accuracy": 5,
    ///   "country": "Germany",
    ///   "timezone": "Europe/Berlin",
    ///   "city": "Düsseldorf",
    ///   "organization": "AS3320 Deutsche Telekom AG",
    ///   "asn": 3320,
    ///   "ip": "87.146.129.31",
    ///   "area_code": "0",
    ///   "organization_name": "Deutsche Telekom AG"
    /// }
    /// ```
    /// </remarks>
    [DataContract]
    public class GeoLocation_geojs_ReturnObject
    {
        /// <summary>
        /// Two-letter country code (ISO 3166-1 alpha-2).
        /// </summary>
        [DataMember]
        public string? CountryCode { get; set; }

        /// <summary>
        /// Three-letter country code (ISO 3166-1 alpha-3).
        /// </summary>
        [DataMember]
        public string? CountryCode3 { get; set; }

        /// <summary>
        /// Continent code.
        /// </summary>
        [DataMember]
        public string? ContinentCode { get; set; }

        /// <summary>
        /// Region or state name.
        /// </summary>
        [DataMember]
        public string? Region { get; set; }

        /// <summary>
        /// Latitude coordinate.
        /// </summary>
        [DataMember]
        public double? Latitude { get; set; }

        /// <summary>
        /// Longitude coordinate.
        /// </summary>
        [DataMember]
        public double? Longitude { get; set; }

        /// <summary>
        /// Reported accuracy of the location data.
        /// </summary>
        [DataMember]
        public int? Accuracy { get; set; }

        /// <summary>
        /// Full country name.
        /// </summary>
        [DataMember]
        public string? Country { get; set; }

        /// <summary>
        /// Time zone identifier.
        /// </summary>
        [DataMember]
        public string? Timezone { get; set; }

        /// <summary>
        /// City name.
        /// </summary>
        [DataMember]
        public string? City { get; set; }

        /// <summary>
        /// Organization name associated with the IP address.
        /// </summary>
        [DataMember]
        public string? Organization { get; set; }

        /// <summary>
        /// Autonomous System Number (ASN) associated with the IP address.
        /// </summary>
        [DataMember]
        public int? Asn { get; set; }

        /// <summary>
        /// IP address used for the geolocation lookup.
        /// </summary>
        [DataMember]
        public string? Ip { get; set; }

        /// <summary>
        /// Area code associated with the IP address (may not be available).
        /// </summary>
        [DataMember]
        public string? AreaCode { get; set; }

        /// <summary>
        /// Full organization name associated with the IP address (may differ from Organization).
        /// </summary>
        [DataMember]
        public string? OrganizationName { get; set; }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public GeoLocation_geojs_ReturnObject()
        {
        }

        /// <summary>
        /// Deserialisierungs-Konstruktor.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Übertragungskontext.</param>
        protected GeoLocation_geojs_ReturnObject(SerializationInfo info, StreamingContext context)
        {
            this.CountryCode = info.GetString("CountryCode");
            this.CountryCode3 = info.GetString("CountryCode3");
            this.ContinentCode = info.GetString("ContinentCode");
            this.Region = info.GetString("Region");
            this.Latitude = (double?)info.GetValue("Latitude", typeof(double));
            this.Longitude = (double?)info.GetValue("Longitude", typeof(double));
            this.Accuracy = (int?)info.GetValue("Accuracy", typeof(int));
            this.Country = info.GetString("Country");
            this.Timezone = info.GetString("Timezone");
            this.City = info.GetString("City");
            this.Organization = info.GetString("Organization");
            this.Asn = (int?)info.GetValue("Asn", typeof(int));
            this.Ip = info.GetString("Ip");
            this.AreaCode = info.GetString("AreaCode");
            this.OrganizationName = info.GetString("OrganizationName");
        }

        /// <summary>
        /// Serialisierungs-Hilfsroutine: holt die Objekt-Properties in den Property-Container.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Serialisierungs-Kontext.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CountryCode", this.CountryCode);
            info.AddValue("CountryCode3", this.CountryCode3);
            info.AddValue("ContinentCode", this.ContinentCode);
            info.AddValue("Region", this.Region);
            info.AddValue("Latitude", this.Latitude);
            info.AddValue("Longitude", this.Longitude);
            info.AddValue("Accuracy", this.Accuracy);
            info.AddValue("Country", this.Country);
            info.AddValue("Timezone", this.Timezone);
            info.AddValue("City", this.City);
            info.AddValue("Organization", this.Organization);
            info.AddValue("Asn", this.Asn);
            info.AddValue("Ip", this.Ip);
            info.AddValue("AreaCode", this.AreaCode);
            info.AddValue("OrganizationName", this.OrganizationName);
        }

        /// <summary>
        /// Überschriebene ToString()-Methode - stellt alle öffentlichen Properties
        /// als einen aufbereiteten String zur Verfügung.
        /// </summary>
        /// <returns>Alle öffentlichen Properties als ein String aufbereitet.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("CountryCode: " + this.CountryCode);
            string delimiter = Environment.NewLine;
            stringBuilder.Append(delimiter + "CountryCode3: " + this.CountryCode3);
            stringBuilder.Append(delimiter + "ContinentCode: " + this.ContinentCode);
            stringBuilder.Append(delimiter + "Region: " + this.Region);
            stringBuilder.Append(delimiter + "Latitude: " + this.Latitude);
            stringBuilder.Append(delimiter + "Longitude: " + this.Longitude);
            stringBuilder.Append(delimiter + "Accuracy: " + this.Accuracy);
            stringBuilder.Append(delimiter + "Country: " + this.Country);
            stringBuilder.Append(delimiter + "Timezone: " + this.Timezone);
            stringBuilder.Append(delimiter + "City: " + this.City);
            stringBuilder.Append(delimiter + "Organization: " + this.Organization);
            stringBuilder.Append(delimiter + "Asn: " + this.Asn);
            stringBuilder.Append(delimiter + "Ip: " + this.Ip);
            stringBuilder.Append(delimiter + "AreaCode: " + this.AreaCode);
            stringBuilder.Append(delimiter + "OrganizationName: " + this.OrganizationName);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Vergleicht dieses Objekt mit einem übergebenen Objekt nach Inhalt.
        /// </summary>
        /// <param name="obj">Das zu vergleichende GeoLocation_ReturnObject.</param>
        /// <returns>True, wenn das übergebene GeoLocation_ReturnObject inhaltlich
        /// gleich diesem GeoLocation_ReturnObject ist.</returns>
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

        internal static GeoLocation_ReturnObject? ConvertTo_GeoLocation_ReturnObject(GeoLocation_geojs_ReturnObject? deserializedLocation)
        {
            GeoLocation_ReturnObject geoLocation_ReturnObject = new GeoLocation_ReturnObject();
            geoLocation_ReturnObject.Country = deserializedLocation?.Country;
            geoLocation_ReturnObject.Region = deserializedLocation?.Region;
            geoLocation_ReturnObject.City = deserializedLocation?.City;
            geoLocation_ReturnObject.Latitude = deserializedLocation?.Latitude;
            geoLocation_ReturnObject.Longitude = deserializedLocation?.Longitude;
            return geoLocation_ReturnObject;
        }
    }
}
