
#if WINDOWS_APP
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Bing.Maps;
#endif

namespace PanoramioTestApp.Controls.Maps
{
    public static class MapExtensions
    {
#if WINDOWS_APP

        public static LocationCollection ToLocationCollection(this IList<BasicGeoposition> pointList)
        {
            var locs = new LocationCollection();

            foreach (var p in pointList)
            {
                locs.Add(p.ToLocation());
            }

            return locs;
        }

        public static Geopoint ToGeopoint(this Location location)
        {
            return new Geopoint(new BasicGeoposition() { Latitude = location.Latitude, Longitude = location.Longitude });
        }

        public static Location ToLocation(this Geopoint location)
        {
            return new Location(location.Position.Latitude, location.Position.Longitude);
        }

        public static Location ToLocation(this BasicGeoposition location)
        {
            return new Location(location.Latitude, location.Longitude);
        }

#elif WINDOWS_PHONE_APP

        //Add any required Windows Phone Extensions

#endif
    }
}