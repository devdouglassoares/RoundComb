using Core.Events;
using Core.UploadService;
using Core.UploadService.FileSystem;
using ImageProcessor;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageProcessor.Imaging;

namespace Upload.Api.Services
{
	public class ImagePostUploadProcessing : IConsumer<FileUploaded>, IConsumer<NonExistsImageDimessionFileRequested>
	{
		public int Order => 10;

		private readonly string[] _supportedFileTypes = { ".jpg", ".jpeg", ".png", ".gif" };

		private readonly int[] _supportedImageWidths = { 240, 320, 480, 640, 1024 };

		private readonly IStorageProvider _storageProvider;

		public ImagePostUploadProcessing(IStorageProvider storageProvider)
		{
			_storageProvider = storageProvider;
		}

		public void HandleEvent(FileUploaded eventMessage)
		{
			if (_supportedFileTypes.All(x => !eventMessage.FileName.EndsWith(x)))
				return;

			foreach (var supportedImageWidth in _supportedImageWidths)
			{
				var imageBytes = _storageProvider.GetFile(eventMessage.FilePath).ReadAllBytes();
				var filePath =
					_storageProvider.Combine(Path.Combine(eventMessage.FolderPath, supportedImageWidth.ToString()),
											 eventMessage.FileName);

				using (var file = Resize(imageBytes, supportedImageWidth, 0))
				{
					_storageProvider.SaveStream(filePath, file);
				}
			}
		}
		public void HandleEvent(NonExistsImageDimessionFileRequested eventMessage)
		{
			var imageBytes = eventMessage.OriginalFile.ReadAllBytes();

			var filePath =
				_storageProvider.Combine(Path.Combine(eventMessage.OriginalFile.GetParentDirectoryName(), eventMessage.RequestedWidth.ToString()),
										 eventMessage.OriginalFile.GetName());

			using (var file = Resize(imageBytes, eventMessage.RequestedWidth, 0))
			{
				_storageProvider.SaveStream(filePath, file);
			}
		}

		private MemoryStream Resize(byte[] photoBytes, int width, int height)
		{
			Size size = new Size(width, height);

			using (MemoryStream inStream = new MemoryStream(photoBytes))
			{
				MemoryStream outStream = new MemoryStream();
				using (ImageFactory imageFactory = new ImageFactory())
				{
					imageFactory.Load(inStream);

					if (imageFactory.Image.Width < width || imageFactory.Image.Height < height)
					{
						imageFactory.Save(outStream);
					}
					else
					{
						imageFactory.Resize(new ResizeLayer(size, ResizeMode.Max))
						            .Quality(100)
									.AutoRotate()
									.Save(outStream);

					}
				}
				return outStream;
			}
		}
	}
}