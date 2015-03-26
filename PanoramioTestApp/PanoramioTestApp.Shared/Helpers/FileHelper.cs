using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace PanoramioTestApp.Helpers
{
    public static class FileHelper
    {
        public static async Task SaveFile(StorageFile file, Stream stream)
        {
            var fileStream = await file.OpenStreamForWriteAsync();
            await stream.CopyToAsync(fileStream);
            await fileStream.FlushAsync();

            fileStream.Dispose();
            stream.Dispose();
        }
    }
}
