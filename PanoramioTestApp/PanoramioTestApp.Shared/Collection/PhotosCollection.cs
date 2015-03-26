using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using PanoramioLib;
using PanoramioTestApp.Common;
using PanoramioTestApp.Services;

namespace PanoramioTestApp.Collection
{
    public class PhotosCollection : IncrementalLoadingBase<PanoramioPhoto>
    {
        private int _totalCount = -1;

        public PhotosCollection()
        {

        }

        public PhotosCollection(IList<PanoramioPhoto> list)
            : base(list)
        {

        }

        public BasicGeoposition Location { get; set; }

        protected override async Task<IList<PanoramioPhoto>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count)
        {
            try
            {
                var photosResponse = await DataService.GetPhotos(Location, PhotoSize.Medium, from: Count, to: Count + (int)count);
                if (photosResponse != null)
                {
                    if (photosResponse.HasMore)
                        _totalCount = photosResponse.Count;
                    else
                        _totalCount = Count + photosResponse.Photos.Count;

                    return photosResponse.Photos;
                }
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);
            }

            return null;
        }

        protected override bool HasMoreItemsOverride()
        {
            return _totalCount == -1 || _totalCount > Count;
        }
    }
}
