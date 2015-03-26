using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace PanoramioTestApp.Helpers
{
    /// <summary>
    /// Вспомогательный класс для работы с геолокацией
    /// </summary>
    public static class GeoHelper
    {
        private static readonly Geolocator _geolocator;

        static GeoHelper()
        {
            _geolocator = new Geolocator();
        }

        /// <summary>
        /// Возвращает текущую геопозицию. Если позицию получить не удалось, вернет координаты 0,0
        /// </summary>
        /// <returns></returns>
        public static async Task<BasicGeoposition> GetCurrentLocation()
        {
            if (_geolocator.LocationStatus != PositionStatus.Disabled)
            {
                try
                {

                    var position = await _geolocator.GetGeopositionAsync();
                    if (position.Coordinate != null && position.Coordinate.Point != null)
                        return position.Coordinate.Point.Position;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return new BasicGeoposition();
        }
    }
}
