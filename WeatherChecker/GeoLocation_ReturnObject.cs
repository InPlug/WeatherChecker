using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace WeatherChecker
{
    /// <summary>
    /// Class to receive the return from the api-call to https://www.geojs.io/.
    /// </summary>
    /// <remarks>
    /// 28.04.2024 Erik Nagel: erstellt
    /// </remarks>
    [DataContract]
    public class GeoLocation_ReturnObject
    {
        /// <summary>
        /// Full country name.
        /// </summary>
        [DataMember]
        public string? Country { get; set; }

        /// <summary>
        /// Region or state name.
        /// </summary>
        [DataMember]
        public string? Region { get; set; }

        /// <summary>
        /// City name.
        /// </summary>
        [DataMember]
        public string? City { get; set; }

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
        /// Standard constructor.
        /// </summary>
        public GeoLocation_ReturnObject()
        {
        }

        /// <summary>
        /// Deserialisierungs-Konstruktor.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Übertragungskontext.</param>
        protected GeoLocation_ReturnObject(SerializationInfo info, StreamingContext context)
        {
            this.Country = info.GetString("Country");
            this.Region = info.GetString("Region");
            this.City = info.GetString("City");
            this.Latitude = (double?)info.GetValue("Latitude", typeof(double));
            this.Longitude = (double?)info.GetValue("Longitude", typeof(double));
        }

        /// <summary>
        /// Serialisierungs-Hilfsroutine: holt die Objekt-Properties in den Property-Container.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Serialisierungs-Kontext.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Region", this.Region);
            info.AddValue("Country", this.Country);
            info.AddValue("City", this.City);
            info.AddValue("Latitude", this.Latitude);
            info.AddValue("Longitude", this.Longitude);
        }

        /// <summary>
        /// Überschriebene ToString()-Methode - stellt alle öffentlichen Properties
        /// als einen aufbereiteten String zur Verfügung.
        /// </summary>
        /// <returns>Alle öffentlichen Properties als ein String aufbereitet.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("Country: " + this.Country);
            string delimiter = Environment.NewLine;
            stringBuilder.Append(delimiter + "Region: " + this.Region);
            stringBuilder.Append(delimiter + "City: " + this.City);
            stringBuilder.Append(delimiter + "Latitude: " + this.Latitude);
            stringBuilder.Append(delimiter + "Longitude: " + this.Longitude);
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
    }
}
