using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WeatherChecker
{
    /// <summary>
    /// ReturnObject für das Ergebnis des WeatherCheckers.
    /// </summary>
    /// <remarks>
    /// Autor: Erik Nagel, NetEti
    ///
    /// 12.10.2022 Erik Nagel: erstellt
    /// </remarks>
    [DataContract]//[Serializable()]
    public class WeatherChecker_ReturnObject
    {
        /// <summary>
        /// Represents the units of measurement for various weather-related metrics.
        /// </summary>
        [DataContract]//[Serializable]
        public class Units : ISerializable, IEquatable<Units>
        {
            /// <summary>
            /// Gets or sets the unit of measurement for sunshine duration.
            /// </summary>
            [DataMember]
            public string? SunshineDuration { get; set; }

            /// <summary>
            /// Gets or sets the unit of measurement for temperature.
            /// </summary>
            [DataMember]
            public string? Temperature { get; set; }

            /// <summary>
            /// Gets or sets the unit of measurement for humidity.
            /// </summary>
            [DataMember]
            public string? Humidity { get; set; }

            /// <summary>
            /// Gets or sets the unit of measurement for surface pressure.
            /// </summary>
            [DataMember]
            public string? SurfacePressure { get; set; }

            /// <summary>
            /// Gets or sets the unit of measurement for rain.
            /// </summary>
            [DataMember]
            public string? Rain { get; set; }

            /// <summary>
            /// Gets or sets the unit of measurement for snowfall.
            /// </summary>
            [DataMember]
            public string? Snowfall { get; set; }

            /// <summary>
            /// Gets or sets the unit of measurement for wind speed at 10 meters.
            /// </summary>
            [DataMember]
            public string? Wind10mSpeed { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Units"/> class.
            /// </summary>
            public Units()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Units"/> class during deserialization.
            /// </summary>
            /// <param name="info">The <see cref="SerializationInfo"/> containing the serialized object data.</param>
            /// <param name="context">The <see cref="StreamingContext"/> that describes the source and destination of the serialized stream.</param>
            protected Units(SerializationInfo info, StreamingContext context)
            {
                SunshineDuration = info.GetString(nameof(SunshineDuration));
                Temperature = info.GetString(nameof(Temperature));
                Humidity = info.GetString(nameof(Humidity));
                SurfacePressure = info.GetString(nameof(SurfacePressure));
                Rain = info.GetString(nameof(Rain));
                Snowfall = info.GetString(nameof(Snowfall));
                Wind10mSpeed = info.GetString(nameof(Wind10mSpeed));
            }

            /// <summary>
            /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
            /// </summary>
            /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
            /// <param name="context">The destination context for this serialization.</param>
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue(nameof(SunshineDuration), SunshineDuration);
                info.AddValue(nameof(Temperature), Temperature);
                info.AddValue(nameof(Humidity), Humidity);
                info.AddValue(nameof(SurfacePressure), SurfacePressure);
                info.AddValue(nameof(Rain), Rain);
                info.AddValue(nameof(Snowfall), Snowfall);
                info.AddValue(nameof(Wind10mSpeed), Wind10mSpeed);
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string representation of the object.</returns>
            public override string ToString()
            {
                return $"SunshineDuration: {SunshineDuration}, Temperature: {Temperature},"
                    + $" Humidity: {Humidity}, SurfacePressure: {SurfacePressure},"
                    + $" Rain: {Rain}, Snowfall: {Snowfall}, Wind10mSpeed: {Wind10mSpeed}";
            }

            /// <summary>
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
            public override bool Equals(object? obj)
            {
                return Equals(obj as Units);
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
            public bool Equals(Units? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;

                return SunshineDuration == other.SunshineDuration &&
                       Temperature == other.Temperature &&
                       Humidity == other.Humidity &&
                       SurfacePressure == other.SurfacePressure &&
                       Rain == other.Rain &&
                       Snowfall == other.Snowfall &&
                       Wind10mSpeed == other.Wind10mSpeed;
            }

            /// <summary>
            /// Serves as the default hash function.
            /// </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode()
            {
                return HashCode.Combine(SunshineDuration, Temperature, Humidity,
                    SurfacePressure, Rain, Snowfall, Wind10mSpeed);
            }
        }

        /// <summary>
        /// Eine Wettervorhersage für einen bestimmten Zeitabschnitt.
        /// </summary>
        [DataContract]//[Serializable()]
        public class ForecastDataPoint
        {
            /// <summary>
            /// Unterklasse: fasst Windrichtung und Windgeschwindigkeit zusammen.
            /// </summary>
            [DataContract]//[Serializable()]
            public class Wind
            {
                /// <summary>
                /// Windrichtung: N, NNE, NE, ENE, E, ESE, SE, SSE, S, SSW, SW, WSW, W, WNW, NW, NNW
                /// </summary>
                [DataMember]
                public string? Direction { get; set; }

                /// <summary>
                /// Windgeschwindigkeit:
                ///     1	unter 0.3m/s    (ruhig)
                ///     2	0.3-3.4m/s      (leicht)
                ///     3	3.4-8.0m/s      (gemäßigt)
                ///     4	8.0-10.8m/s     (frisch)
                ///     5	10.8-17.2m/s    (stark)
                ///     6	17.2-24.5m/s    (heftig)
                ///     7	24.5-32.6m/s    (stürmisch)
                ///     8	über 32.6m/s    (Orkan)
                /// </summary>
                [DataMember]
                public string? Speed { get; set; }

                /// <summary>
                /// Windgeschwindigkeit:
                ///     1	unter 0.3m/s    (ruhig)
                ///     2	0.3-3.4m/s      (leicht)
                ///     3	3.4-8.0m/s      (gemäßigt)
                ///     4	8.0-10.8m/s     (frisch)
                ///     5	10.8-17.2m/s    (stark)
                ///     6	17.2-24.5m/s    (heftig)
                ///     7	24.5-32.6m/s    (stürmisch)
                ///     8	über 32.6m/s    (Orkan)
                /// </summary>
                [DataMember]
                public string? GustsSpeed { get; set; }

                /// <summary>
                /// Standard Konstruktor.
                /// </summary>
                public Wind() { }

                /// <summary>
                /// Deserialisierungs-Konstruktor.
                /// </summary>
                /// <param name="info">Property-Container.</param>
                /// <param name="context">Übertragungs-Kontext.</param>
                protected Wind(SerializationInfo info, StreamingContext context)
                {
                    this.Direction = info.GetString("Direction");
                    this.Speed = info.GetString("Speed");
                    this.GustsSpeed = info.GetString("GustsSpeed");
                }

                /// <summary>
                /// Serialisierungs-Hilfsroutine: holt die Objekt-Properties in den Property-Container.
                /// </summary>
                /// <param name="info">Property-Container.</param>
                /// <param name="context">Serialisierungs-Kontext.</param>
                public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
                {
                    info.AddValue("Direction", this.Direction);
                    info.AddValue("Speed", this.Speed);
                    info.AddValue("GustsSpeed", this.GustsSpeed);
                }

                /// <summary>
                /// Überschriebene ToString()-Methode.
                /// </summary>
                /// <returns>Dieses Objekt.ToString().</returns>
                public override string ToString()
                {
                    StringBuilder stringBuilder = new StringBuilder(this.Direction);
                    string delimiter = ", ";
                    stringBuilder.Append(delimiter + this.Speed);
                    stringBuilder.Append(delimiter + this.GustsSpeed);
                    return stringBuilder.ToString();
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
            /// Definiert den Aufsetz-Zeitpunkt ("2024-04-28T00:00").
            /// </summary>
            [DataMember]
            public string? Timepoint { get; set; }

            /// <summary>
            /// Temperatur (in 2 m Höhe über dem Erdboden gemessen): -76 to 60: -76C to +60C.
            /// </summary>
            [DataMember]
            public string? Temperature { get; set; }

            /// <summary>
            /// Relative Luftfeuchtigkeit (in 2 m Höhe über dem Erdboden gemessen): 0 to 100: 0% to 100%.
            /// </summary>
            [DataMember]
            public string? Humidity { get; set; }

            /// <summary>
            /// The unit of measurement for the apparent temperature data (e.g., "celsius").
            /// </summary>
            [DataMember]
            public string? ApparentTemperature { get; set; }

            /// <summary>
            /// List of precipitation probability values (from HourlyUnits.PrecipitationProbability).
            /// </summary>
            [DataMember]
            public string? PrecipitationProbability { get; set; }

            /// <summary>
            /// List of rain values (from HourlyUnits.Rain).
            /// </summary>
            [DataMember]
            public string? Rain { get; set; }

            /// <summary>
            /// List of showers values (from HourlyUnits.Showers).
            /// </summary>
            [DataMember]
            public string? Showers { get; set; }

            /// <summary>
            /// List of snowfall values (from HourlyUnits.Snowfall).
            /// </summary>
            [DataMember]
            public string? Snowfall { get; set; }

            /// <summary>
            /// Wetterbeschreibung:
            ///     clearday, clearnight	        Total cloud cover less than 20%
            ///     pcloudyday, pcloudynight        Total cloud cover between 20%-60%
            ///     mcloudyday, mcloudynight        Total cloud cover between 60%-80%
            ///     cloudyday, cloudynight          Total cloud cover over over 80%
            ///     humidday, humidnight            Relative humidity over 90% with total cloud cover less than 60%
            ///     lightrainday, lightrainnight    Precipitation rate less than 4mm/hr with total cloud cover more than 80%
            ///     oshowerday, oshowernight        Precipitation rate less than 4mm/hr with total cloud cover between 60%-80%
            ///     ishowerday, ishowernight        Precipitation rate less than 4mm/hr with total cloud cover less than 60%
            ///     lightsnowday, lightsnownight    Precipitation rate less than 4mm/hr
            ///     rainday, rainnight              Precipitation rate over 4mm/hr
            ///     snowday, snownight              Precipitation rate over 4mm/hr
            ///     rainsnowday, rainsnownight      Precipitation type to be ice pellets or freezing rain.
            /// </summary>
            [DataMember]
            public string? Weather { get; set; }

            /// <summary>
            /// List of surface pressure data (from HourlyUnits.SurfacePressure).
            /// </summary>
            [DataMember]
            public string? SurfacePressure { get; set; }

            /// <summary>
            /// Bewölkung:
            ///     1	0%-6%
            ///     2	6%-19%
            ///     3	19%-31%
            ///     4	31%-44%
            ///     5	44%-56%
            ///     6	56%-69%
            ///     7	69%-81%
            ///     8	81%-94%
            ///     9	94%-100%
            /// </summary>
            [DataMember]
            public string? Cloudcover { get; set; }

            /// <summary>
            /// Unterklasse: fasst Windrichtung und Windgeschwindigkeit zusammen.
            /// </summary>
            [DataMember]
            public Wind? Wind10m { get; set; }

            /// <summary>
            /// List of sunrise times for each day (in the unit specified by DailyUnits.Sunrise).
            /// </summary>
            [DataMember]
            public string? Sunrise { get; set; }

            /// <summary>
            /// List of sunset times for each day (in the unit specified by DailyUnits.Sunset).
            /// </summary>
            [DataMember]
            public string? Sunset { get; set; }

            /// <summary>
            /// List of sunshine duration times for each day (in the unit specified by DailyUnits.SunshineDuration).
            /// </summary>
            [DataMember]
            public string? SunshineDuration { get; set; }

            /// <summary>
            /// List of rain sum values (from DailyUnits.RainSum).
            /// </summary>
            [DataMember]
            public string? RainSum { get; set; }

            /// <summary>
            /// List of showers sum values (from DailyUnits.ShowersSum).
            /// </summary>
            [DataMember]
            public string? ShowersSum { get; set; }

            /// <summary>
            /// List of snowfall sum values (from DailyUnits.SnowfallSum).
            /// </summary>
            [DataMember]
            public string? SnowfallSum { get; set; }

            /// <summary>
            /// Standard Konstruktor.
            /// </summary>
            public ForecastDataPoint() { }

            /// <summary>
            /// Deserialisierungs-Konstruktor.
            /// </summary>
            /// <param name="info">Property-Container.</param>
            /// <param name="context">Übertragungs-Kontext.</param>
            protected ForecastDataPoint(SerializationInfo info, StreamingContext context)
            {
                this.Timepoint = info.GetString("Timepoint");
                this.Temperature = info.GetString("Temperature");
                this.Humidity = info.GetString("Humidity");
                this.ApparentTemperature = info.GetString("ApparentTemperature");
                this.PrecipitationProbability = info.GetString("PrecipitationProbability");
                this.Rain = info.GetString("Rain");
                this.Showers = info.GetString("Showers");
                this.Snowfall = info.GetString("Snowfall");
                this.Weather = info.GetString("Weather");
                this.SurfacePressure = info.GetString("SurfacePressure");
                this.Cloudcover = info.GetString("Cloudcover");
                this.Wind10m = (Wind?)info.GetValue("Wind10m", typeof(Wind));
                this.Sunrise = info.GetString("Sunrise");
                this.Sunset = info.GetString("Sunset");
                this.SunshineDuration = info.GetString("SunshineDuration");
                this.RainSum = info.GetString("RainSum");
                this.ShowersSum = info.GetString("ShowersSum");
                this.SnowfallSum = info.GetString("SnowfallSum");
            }

            /// <summary>
            /// Serialisierungs-Hilfsroutine: holt die Objekt-Properties in den Property-Container.
            /// </summary>
            /// <param name="info">Property-Container.</param>
            /// <param name="context">Serialisierungs-Kontext.</param>
            public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Timepoint", this.Timepoint);
                info.AddValue("Temperature", this.Temperature);
                info.AddValue("Humidity", this.Humidity);
                info.AddValue("ApparentTemperature", this.ApparentTemperature);
                info.AddValue("PrecipitationProbability", this.PrecipitationProbability);
                info.AddValue("Rain", this.Rain);
                info.AddValue("Showers", this.Showers);
                info.AddValue("Snowfall", this.Snowfall);
                info.AddValue("Weather", this.Weather);
                info.AddValue("SurfacePressure", this.SurfacePressure);
                info.AddValue("Cloudcover", this.Cloudcover);
                info.AddValue("Wind10m", this.Wind10m);
                info.AddValue("Sunrise", this.Sunrise);
                info.AddValue("Sunset", this.Sunset);
                info.AddValue("SunshineDuration", this.SunshineDuration);
                info.AddValue("RainSum", this.RainSum);
                info.AddValue("ShowersSum", this.ShowersSum);
                info.AddValue("SnowfallSum", this.SnowfallSum);
            }

            /// <summary>
            /// Überschriebene ToString()-Methode.
            /// </summary>
            /// <returns>Dieses Objekt.ToString().</returns>
            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder(this.Timepoint?.ToString());
                string delimiter = ", ";
                stringBuilder.Append(delimiter + this.Temperature);
                stringBuilder.Append(delimiter + this.Humidity);
                stringBuilder.Append(delimiter + this.ApparentTemperature);
                stringBuilder.Append(delimiter + this.PrecipitationProbability);
                stringBuilder.Append(delimiter + this.Rain);
                stringBuilder.Append(delimiter + this.Showers);
                stringBuilder.Append(delimiter + this.Snowfall);
                stringBuilder.Append(delimiter + this.Weather);
                stringBuilder.Append(delimiter + this.SurfacePressure);
                stringBuilder.Append(delimiter + this.Cloudcover);
                stringBuilder.Append(delimiter + this.Wind10m?.ToString());
                stringBuilder.Append(delimiter + this.Sunrise?.ToString());
                stringBuilder.Append(delimiter + this.Sunset?.ToString());
                stringBuilder.Append(delimiter + this.SunshineDuration?.ToString());
                stringBuilder.Append(delimiter + this.RainSum?.ToString());
                stringBuilder.Append(delimiter + this.ShowersSum?.ToString());
                stringBuilder.Append(delimiter + this.SnowfallSum?.ToString());
                return stringBuilder.ToString();
            }

            /// <summary>
            /// Vergleicht dieses Objekt mit einem übergebenen Objekt nach Inhalt.
            /// </summary>
            /// <param name="obj">Der zu vergleichende ForecastDataPoint.</param>
            /// <returns>True, wenn der übergebene ForecastDataPoint inhaltlich gleich diesem ForecastDataPoint ist.</returns>
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
        /// Datum und Stunde der Initialisierung.
        /// </summary>
        [DataMember]
        public string? Creation { get; set; }

        /// <summary>
        /// Enthält Ortsinformationen.
        /// </summary>
        [DataMember]
        public Units? UnitsContainer{ get; set; }

        /// <summary>
        /// Enthält Ortsinformationen.
        /// </summary>
        [DataMember]
        public GeoLocation_ReturnObject? Location { get; set; }

        /// <summary>
        /// Array von Wettervorhersagen für aufeinander folgende Zeitabschnitte.
        /// </summary>
        [DataMember]
        public List<ForecastDataPoint>? Dataseries { get; set; }

        /// <summary>
        /// Liefert die Anzahl ForecastDataPoints.
        /// </summary>
        [DataMember]
        public int? RecordCount
        {
            get
            {
                if (this.Dataseries != null)
                {
                    return this.Dataseries.Count;
                }
                return 0;
            }
        }

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        public WeatherChecker_ReturnObject()
        {
            this.Creation = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            this.UnitsContainer = new Units()
            {
                Humidity = ".%",
                Rain = ".mm",
                Snowfall = ".cm",
                SunshineDuration = ".s",
                Temperature = ".°C",
                Wind10mSpeed = ".km/h"
            };
            this.Dataseries = new List<ForecastDataPoint>();
        }

        /// <summary>
        /// Deserialisierungs-Konstruktor.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Übertragungs-Kontext.</param>
        protected WeatherChecker_ReturnObject(SerializationInfo info, StreamingContext context)
        {
            this.Creation = info.GetString("Creation");
            this.UnitsContainer = (Units?)info.GetValue("UnitsContainer", typeof(Units));
            this.Location = (GeoLocation_ReturnObject?)info.GetValue("Location", typeof(GeoLocation_ReturnObject));
            this.Dataseries = (List<ForecastDataPoint>?)info.GetValue("Dataseries", typeof(List<ForecastDataPoint>));
        }

        /// <summary>
        /// Serialisierungs-Hilfsroutine: holt die Objekt-Properties in den Property-Container.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Serialisierungs-Kontext.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Creation", this.Creation);
            info.AddValue("UnitsContainer", this.UnitsContainer);
            info.AddValue("Location", this.Location);
            info.AddValue("Dataseries", this.Dataseries);
        }

        /// <summary>
        /// Überschriebene ToString()-Methode - stellt alle öffentlichen Properties
        /// als einen aufbereiteten String zur Verfügung.
        /// </summary>
        /// <returns>Alle öffentlichen Properties als ein String aufbereitet.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(this.Location?.City + " " + this.Location?.Region);
            string delimiter = ", ";
            stringBuilder.Append(delimiter + this.Creation + Environment.NewLine);
            if (this.Dataseries != null)
            {
                foreach (ForecastDataPoint forecastDataPoint in this.Dataseries)
                {
                    stringBuilder.Append(delimiter + forecastDataPoint.ToString());
                    delimiter = Environment.NewLine;
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Vergleicht dieses Objekt mit einem übergebenen Objekt nach Inhalt.
        /// </summary>
        /// <param name="obj">Das zu vergleichende WeatherChecker_ReturnObject.</param>
        /// <returns>True, wenn das übergebene WeatherChecker_ReturnObject inhaltlich gleich diesem WeatherChecker_ReturnObject ist.</returns>
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
            WeatherChecker_ReturnObject subResultListContainer = (WeatherChecker_ReturnObject)obj;
            if (this.Dataseries?.Count != subResultListContainer.Dataseries?.Count)
            {
                return false;
            }
            for (int i = 0; i < this.Dataseries?.Count; i++)
            {
                if (this.Dataseries[i] != subResultListContainer.Dataseries?[i])
                {
                    return false;
                }
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
}
