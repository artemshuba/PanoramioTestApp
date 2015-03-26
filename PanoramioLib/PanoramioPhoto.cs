using Newtonsoft.Json;

namespace PanoramioLib
{
    /// <summary>
    /// Panoramio photo
    /// </summary>
    public class PanoramioPhoto
    {
        /// <summary>
        /// Photo id
        /// </summary>
        [JsonProperty("photo_id")]
        public int PhotoId { get; set; }

        /// <summary>
        /// Photo title
        /// </summary>
        [JsonProperty("photo_title")]
        public string PhotoTitle { get; set; }

        /// <summary>
        /// Photo url
        /// </summary>
        [JsonProperty("photo_url")]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Photo file url
        /// </summary>
        [JsonProperty("photo_file_url")]
        public string PhotoFileUrl { get; set; }

        /// <summary>
        /// Original photo file url
        /// </summary>
        public string PhotoOriginalFileUrl
        {
            get { return string.Format(Panoramio.OriginalPhotoUrl, PhotoId); }
        }

        /// <summary>
        /// Longitude
        /// </summary>
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        /// <summary>
        /// Photo width
        /// </summary>
        [JsonProperty("width")]
        public double Width { get; set; }

        /// <summary>
        /// Photo height
        /// </summary>
        [JsonProperty("height")]
        public double Height { get; set; }

        /// <summary>
        /// Upload date
        /// </summary>
        [JsonProperty("upload_date")]
        public string UploadDate { get; set; }

        /// <summary>
        /// Owner id
        /// </summary>
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        /// <summary>
        /// Owner name
        /// </summary>
        [JsonProperty("owner_name")]
        public string OwnerName { get; set; }

        /// <summary>
        /// Owner url
        /// </summary>
        [JsonProperty("owner_url")]
        public string OwnerUrl { get; set; }
    }
}
