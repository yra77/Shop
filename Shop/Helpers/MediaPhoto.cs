
#if !WINDOWS
using Microsoft.Maui.Graphics.Platform;
#endif

namespace Shop.Helpers
{
    internal class MediaPhoto
    {
        public static async Task<byte[]> TakePhoto()// save the file into local storage
        {
#if !WINDOWS
            string photoPath = null;

            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();// CapturePhotoAsync();

                if (photo != null)
                {
                    photoPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();


                    var image = PlatformImage.FromStream(sourceStream);
                    Microsoft.Maui.Graphics.IImage newImage = image.Downsize(150, 150, false);

                    return newImage.AsBytes();
                }
            }
#endif
            return new byte[0];
        }

        private static byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
