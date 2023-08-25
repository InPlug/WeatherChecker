using Newtonsoft.Json;
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
                /// Windrichtung: N, NE, E, SE, S, SW, W, NW.
                /// </summary>
                [DataMember]
                [JsonProperty("direction")]
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
                [JsonProperty("speed")]
                public int? Speed { get; set; }

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
                    this.Speed = (int?)info.GetValue("Speed", typeof(int));
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
                }

                /// <summary>
                /// Überschriebene ToString()-Methode.
                /// </summary>
                /// <returns>Dieses Objekt.ToString().</returns>
                public override string ToString()
                {
                    StringBuilder stringBuilder = new StringBuilder(this.Direction);
                    string delimiter = ", ";
                    stringBuilder.Append(delimiter + this.Speed.ToString());
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
            /// Definiert den Aufsetz-Zeitpunkt.
            /// </summary>
            [DataMember]
            [JsonProperty("timepoint")]
            public int? Timepoint { get; set; }

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
            [JsonProperty("cloudcover")]
            public int? Cloudcover { get; set; }

            /// <summary>
            /// Stabilitätsindikator - je kleiner, desto instabiler ist die Wetterlage:
            ///     -10	    Below -7
            ///      -6	    -7 to -5
            ///      -4	    -5 to -3
            ///      -1	    -3 to 0
            ///       2	     0 to 4
            ///       6	     4 to 8
            ///      10	     8 to 11
            ///      15	    Over 11
            /// </summary>
            [DataMember]
            [JsonProperty("lifted_index")]
            public int? Lifted_Index { get; set; }

            /// <summary>
            /// Niederschlag: snow, rain, frzr (freezing rain), icep (ice pellets), none.
            /// </summary>
            [DataMember]
            [JsonProperty("prec_type")]
            public string? Prec_Type { get; set; }

            /// <summary>
            /// Niederschlagsmenge:
            ///     0	None
            ///     1    0-0.25mm/hr
            ///     2	 0.25-1mm/hr
            ///     3	 1-4mm/hr
            ///     4	 4-10mm/hr
            ///     5	10-16mm/hr
            ///     6	16-30mm/hr
            ///     7	30-50mm/hr
            ///     8	50-75mm/hr
            ///     9	Over 75mm/hr
            /// </summary>
            [DataMember]
            [JsonProperty("prec_amount")]
            public int? Prec_Amount { get; set; }

            /// <summary>
            /// Temperatur (in 2 m Höhe über dem Erdboden gemessen): -76 to 60: -76C to +60C.
            /// </summary>
            [DataMember]
            [JsonProperty("temp2m")]
            public int? Temp2m { get; set; }

            /// <summary>
            /// Relative Luftfeuchtigkeit (in 2 m Höhe über dem Erdboden gemessen): 0 to 100: 0% to 100%.
            /// </summary>
            [DataMember]
            [JsonProperty("rh2m")]
            public string? Rh2m { get; set; }

            /// <summary>
            /// Unterklasse: fasst Windrichtung und Windgeschwindigkeit zusammen.
            /// </summary>
            [DataMember]
            [JsonProperty("wind10m")]
            public Wind? Wind10m { get; set; }

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
            [JsonProperty("weather")]
            public string? Weather { get; set; }

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
                this.Timepoint = (int?)info.GetValue("Timepoint", typeof(int));
                this.Cloudcover = (int?)info.GetValue("Cloudcover", typeof(int));
                this.Lifted_Index = (int?)info.GetValue("Lifted_Index", typeof(int));
                this.Prec_Type = info.GetString("Prec_Type");
                this.Prec_Amount = (int?)info.GetValue("Prec_Amount", typeof(int));
                this.Temp2m = (int?)info.GetValue("Temp2m", typeof(int));
                this.Rh2m = info.GetString("Rh2m");
                this.Wind10m = (Wind?)info.GetValue("Wind10m", typeof(Wind));
                this.Weather = info.GetString("Weather");
            }

            /// <summary>
            /// Serialisierungs-Hilfsroutine: holt die Objekt-Properties in den Property-Container.
            /// </summary>
            /// <param name="info">Property-Container.</param>
            /// <param name="context">Serialisierungs-Kontext.</param>
            public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Timepoint", this.Timepoint);
                info.AddValue("Cloudcover", this.Cloudcover);
                info.AddValue("Lifted_Index", this.Lifted_Index);
                info.AddValue("Prec_Type", this.Prec_Type);
                info.AddValue("Prec_Amount", this.Prec_Amount);
                info.AddValue("Temp2m", this.Temp2m);
                info.AddValue("Rh2m", this.Rh2m);
                info.AddValue("Wind10m", this.Wind10m);
                info.AddValue("Weather", this.Weather);
            }

            /// <summary>
            /// Überschriebene ToString()-Methode.
            /// </summary>
            /// <returns>Dieses Objekt.ToString().</returns>
            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder(this.Timepoint.ToString());
                string delimiter = ", ";
                stringBuilder.Append(delimiter + this.Cloudcover.ToString());
                stringBuilder.Append(delimiter + this.Lifted_Index.ToString());
                stringBuilder.Append(delimiter + this.Prec_Type);
                stringBuilder.Append(delimiter + this.Prec_Amount.ToString());
                stringBuilder.Append(delimiter + this.Temp2m.ToString());
                stringBuilder.Append(delimiter + this.Rh2m);
                stringBuilder.Append(delimiter + this.Wind10m?.ToString());
                stringBuilder.Append(delimiter + this.Weather);
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
        /// Die ausgewählte Unter-Routine der Api http://www.7timer.info/bin/api.pl.
        /// </summary>
        [DataMember]
        [JsonProperty("product")]
        public string? Product { get; set; }

        /// <summary>
        /// Datum und Stunde der Initialisierung.
        /// </summary>
        [DataMember]
        [JsonProperty("init")]
        public string? Init { get; set; }

        /// <summary>
        /// Array von Wettervorhersagen für aufeinander folgende Zeitabschnitte.
        /// </summary>
        [DataMember]
        [JsonProperty("dataseries")]
        public List<ForecastDataPoint>? Dataseries { get; set; }

        /// <summary>
        /// Enthält Ortsinformationen.
        /// </summary>
        [DataMember]
        public GeoLocation_ReturnObject? Location { get; set; }

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
            this.Dataseries = new List<ForecastDataPoint>();
        }

        /// <summary>
        /// Deserialisierungs-Konstruktor.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Übertragungs-Kontext.</param>
        protected WeatherChecker_ReturnObject(SerializationInfo info, StreamingContext context)
        {
            this.Product = info.GetString("Product");
            this.Product = info.GetString("Init");
            this.Dataseries = (List<ForecastDataPoint>?)info.GetValue("Dataseries", typeof(List<ForecastDataPoint>));
            this.Location = (GeoLocation_ReturnObject?)info.GetValue("Location", typeof(GeoLocation_ReturnObject));
        }

        /// <summary>
        /// Serialisierungs-Hilfsroutine: holt die Objekt-Properties in den Property-Container.
        /// </summary>
        /// <param name="info">Property-Container.</param>
        /// <param name="context">Serialisierungs-Kontext.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Product", this.Product);
            info.AddValue("Init", this.Init);
            info.AddValue("Dataseries", this.Dataseries);
            info.AddValue("Location", this.Location);
        }

        /// <summary>
        /// Überschriebene ToString()-Methode - stellt alle öffentlichen Properties
        /// als einen aufbereiteten String zur Verfügung.
        /// </summary>
        /// <returns>Alle öffentlichen Properties als ein String aufbereitet.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(this.Location?.city + " " + this.Location?.regionName);
            stringBuilder.Append(Environment.NewLine + this.Product);
            string delimiter = ", ";
            stringBuilder.Append(delimiter + this.Init + Environment.NewLine);
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
