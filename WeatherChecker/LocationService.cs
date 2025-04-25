using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Windows_Geolocation
{
    /// <summary>
    /// Klasse zum Ermitteln des Standortes über den Windows Standort (Geolocation-Dienst).
    /// </summary>
    /// <remarks>
    /// Autor: Erik Nagel
    ///
    /// 10.04.2025 Erik Nagel: neu erstellt.
    /// </remarks>
    public class LocationService
    {
        /// <summary>
        /// Fordert den aktuellen Standort des Geräts vom Windows System an.
        /// </summary>
        /// <returns>Eine Task, die ein GeoPosition liefert.</returns>
        /// <exception cref="UnauthorizedAccessException">Exception, wenn der Standortzugriff unter Windows nicht erlaubt ist.</exception>
        public async Task<Geoposition> GetCurrentLocationAsync()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus != GeolocationAccessStatus.Allowed)
            {
                throw new UnauthorizedAccessException("Zugriff auf Standortdaten wurde verweigert.");
            }

            // Instanzieren des Geolocators mit der gewünschten Genauigkeit
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 100 };
            return await geolocator.GetGeopositionAsync();
        }
    }
}
