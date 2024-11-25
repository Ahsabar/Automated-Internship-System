using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;

namespace api.Helpers
{
	public class ImageUploadHandler : IImageUploadHandler
	{
		public async Task<byte[]> UploadFileAsync(IFormFile file)
		{
			List<string> validExtensions = new List<string>() {".jpg", ".png", "jpeg"};
			string extension = Path.GetExtension(file.FileName); 

			if (!validExtensions.Contains(extension))
			{
				throw new ArgumentException("Invalid file.");
			}

			using (var memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}