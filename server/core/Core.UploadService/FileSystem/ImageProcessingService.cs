using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace Core.UploadService.FileSystem
{
    public class ImageProcessingService
    {
        public static Bitmap ConvertImageFromBase64(string imageDataUri)
        {
            var base64Data = Regex.Match(imageDataUri, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            using (var stream = new MemoryStream(binData))
            {
                var image = new Bitmap(stream);
                return image;
            }
        }

        public static void SaveImageFromBase64String(string url, string path, ImageFormat format)
        {
            var base64Data = Regex.Match(url, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            using (var stream = new MemoryStream(binData))
            {
                var image = Image.FromStream(stream);
                image.Save(path, format);
            }
        }

        public static Stream GetImageStreamFromBase64String(string base64Uri, ImageFormat format)
        {
            var base64Data = Regex.Match(base64Uri, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            using (var stream = new MemoryStream(binData))
            {
                var image = Image.FromStream(stream);

                var newMemStream = new MemoryStream();
                image.Save(newMemStream, format);
                newMemStream.Position = 0;
                return newMemStream;
            }
        }
    }
}