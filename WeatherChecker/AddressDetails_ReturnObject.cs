using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WeatherChecker
{
    /// <summary>
    /// Class to receive the return from https://nominatim.openstreetmap.org/reverse.
    /// </summary>
    /// <remarks>
    /// 26.04.2025 Erik Nagel: erstellt
    /// </remarks>
    public class AddressDetails_ReturnObject
    {
        /// <summary>
        /// Full country name.
        /// </summary>
        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        /// <summary>
        /// Copyright license.
        /// </summary>
        [JsonPropertyName("licence")]
        public string? Licence { get; set; }

    }

    /// <summary>
    /// Class containing the postal address.
    /// </summary>
    /// <remarks>
    /// 28.04.2024 Erik Nagel: erstellt
    /// </remarks>
    public class Address
    {
        /// <summary>
        /// Full country name.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Region or state name.
        /// </summary>
        [JsonPropertyName("state")]
        public string? State { get; set; }

        /// <summary>
        /// City name.
        /// </summary>
        [JsonPropertyName("city")]
        public string? City { get; set; }

    }
}
