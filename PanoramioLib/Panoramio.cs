using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Web.Http;
using Newtonsoft.Json;
using PanoramioLib.Utils.Extensions;

namespace PanoramioLib
{
    public class Panoramio
    {
        //Base url for api requests
        private const string _baseUrl = "http://www.panoramio.com/map/{0}.php";

        //Url to photo of original size
        internal const string OriginalPhotoUrl = "http://static.panoramio.com/photos/original/{0}.jpg";

        /// <summary>
        /// Get photos
        /// </summary>
        /// <param name="set">Public (popular photos), full (all photos) or user ID number</param>
        /// <param name="minX">Defines the area to show photos</param>
        /// <param name="minY">Defines the area to show photos</param>
        /// <param name="maxX">Defines the area to show photos</param>
        /// <param name="maxY">Defines the area to show photos</param>
        /// <param name="size">Requsted size</param>
        /// <param name="mapFilter">True - filter photos for better look on map</param>
        /// <param name="from">Offset</param>
        /// <param name="to">To</param>
        /// <returns></returns>
        public async Task<PhotosResponse> GetPhotos(string set, double minX, double minY, double maxX, double maxY,
            PhotoSize size = PhotoSize.Medium, bool mapFilter = true, int from = 0, int to = 20)
        {
            var p = new Dictionary<string, string>();

            p.Add("set", set);
            p.Add("minx", minX.ToString(CultureInfo.InvariantCulture));
            p.Add("miny", minY.ToString(CultureInfo.InvariantCulture));
            p.Add("maxx", maxX.ToString(CultureInfo.InvariantCulture));
            p.Add("maxy", maxY.ToString(CultureInfo.InvariantCulture));

            var sizeString = "medium";
            switch (size)
            {
                case PhotoSize.Original:
                    sizeString = "original";
                    break;

                case PhotoSize.Small:
                    sizeString = "small";
                    break;

                case PhotoSize.Thumbnail:
                    sizeString = "thumbnail";
                    break;

                case PhotoSize.Square:
                    sizeString = "square";
                    break;

                case PhotoSize.MiniSquare:
                    sizeString = "mini_square";
                    break;
            }

            p.Add("size", sizeString);
            p.Add("mapFilter", mapFilter.ToString());

            p.Add("from", from.ToString());

            p.Add("to", to.ToString());

            return await Get<PhotosResponse>(new Uri(string.Format(_baseUrl, "get_panoramas")), p);
        }

        //Get api request
        private async Task<T> Get<T>(Uri uri, Dictionary<string, string> parameters = null)
        {
            var requestUri = uri;

            if (parameters != null)
                requestUri = new Uri(uri.OriginalString + "?" + parameters.ToQueryString());

            Debug.WriteLine("GET " + requestUri.OriginalString);

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(requestUri);

            var responseText = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseText);
        }
    }
}
