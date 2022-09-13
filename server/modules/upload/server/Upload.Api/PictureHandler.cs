using Core.Events;
using Core.UploadService;
using Core.UploadService.FileSystem;
using ImageProcessor;
using Microsoft.Practices.ServiceLocation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Upload.Api
{
	public class PictureHandler : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			var storageProvider = ServiceLocator.Current.GetInstance<IStorageProvider>();

			var requestUrl = context.Request.FilePath;

			var imageFile = storageProvider.GetStoragePath(requestUrl);

			if (!storageProvider.FileExists(imageFile))
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return;
			}

			var storageFile = storageProvider.GetFile(imageFile);
			var file = storageFile.GetFullName();

			var thumbWidthString = context.Request.QueryString.GetValues("width");
			var thumbHeightString = context.Request.QueryString.GetValues("height");

			var thumbWidth = 0;
			if (!string.IsNullOrEmpty(thumbWidthString?.First()))
			{
				int.TryParse(thumbWidthString.First(), out thumbWidth);
			}

			var thumbHeight = 0;
			if (!string.IsNullOrEmpty(thumbHeightString?.First()))
			{
				int.TryParse(thumbHeightString.First(), out thumbHeight);
			}

			context.Response.ContentType = FileInfoExtensions.GetMimeType(storageFile.GetFileType());

			if (thumbHeight == 0)
			{
				if (thumbWidth == 0)
				{
					context.Response.WriteFile(file);
					context.Response.Flush();
					return;
				}

				var fileName = storageFile.GetName();
				var filePath = storageFile.GetDirectory();
				var newPath = Path.Combine(filePath, thumbWidth.ToString(), fileName);
				if (File.Exists(newPath))
				{
					context.Response.WriteFile(newPath);
					context.Response.Flush();
					return;
				}

				var eventPublisher = ServiceLocator.Current.GetInstance<IEventPublisher>();
				eventPublisher.Publish(new NonExistsImageDimessionFileRequested
				{
					RequestedWidth = thumbWidth,
					OriginalFile = storageFile
				});

				if (File.Exists(newPath))
				{
					context.Response.WriteFile(newPath);
					context.Response.Flush();
					return;
				}
			}


			var image = Resize(file, thumbWidth, thumbHeight);

			var buffer = image.ToArray();
			context.Response.BinaryWrite(buffer);
			context.Response.Flush();
		}

		public bool IsReusable => false;

		private MemoryStream Resize(string path, int width, int height)
		{
			byte[] photoBytes = File.ReadAllBytes(path);
			Size size = new Size(width, height);

			using (MemoryStream inStream = new MemoryStream(photoBytes))
			{
				using (MemoryStream outStream = new MemoryStream())
				{
					// Initialize the ImageFactory using the overload to preserve EXIF metadata.
					using (ImageFactory imageFactory = new ImageFactory())
					{
						// Load, resize, set the format and quality and save an image.
						imageFactory.Load(inStream)
						            .AutoRotate()
						            .Quality(100)
						            .Resize(size)
						            .Save(outStream);
					}
					return outStream;
				}
			}
		}
	}
}