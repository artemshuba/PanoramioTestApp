using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using PanoramioLib;

namespace PanoramioTestApp.Services
{
    public static class DataService
    {
        private const double _photoRadius = 0.01f;

        private static readonly Panoramio _panoramio = new Panoramio();

        public static async Task<PhotosResponse> GetPhotos(BasicGeoposition location,
            PhotoSize size = PhotoSize.Medium, bool mapFilter = true, int from = 0, int to = 20)
        {
            return await _panoramio.GetPhotos("full", location.Longitude - _photoRadius / 2,
                    location.Latitude - _photoRadius / 2, location.Longitude + _photoRadius / 2, location.Latitude + _photoRadius / 2,
                    size, mapFilter, from, to);
        }
    }
}
