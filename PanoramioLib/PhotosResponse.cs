using System.Collections.Generic;
using Newtonsoft.Json;

namespace PanoramioLib
{
    /// <summary>
    /// Api photos response
    /// </summary>
    public class PhotosResponse
    {
        /// <summary>
        /// Total count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Has more photos
        /// </summary>
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        /// <summary>
        /// Photos (maximum 100)
        /// </summary>
        public List<PanoramioPhoto> Photos { get; set; }

        public PhotosResponse()
        {
            Photos = new List<PanoramioPhoto>();
        }
    }
}
