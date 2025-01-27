using Million.Technical.Test.Application.Shared.Methods;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Million.Technical.Test.Infrastructure.Shared.Methods
{
    public class ImageService : IImageService
    {
        private readonly string[] _validExtensions = { ".jpg", ".jpeg", ".png" };

        public async Task<byte[]> CompressImageAsync(byte[] imageData, string fileName)
        {
            if (!IsValidImageFile(fileName))
            {
                throw new ArgumentException("Invalid file extension.");
            }

            try
            {
                using var image = Image.Load(imageData);

                if (image.Width > 1920 || image.Height > 1080)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(1920, 1080),
                        Mode = ResizeMode.Max
                    }));
                }

                using var ms = new MemoryStream();

                await image.SaveAsJpegAsync(ms, new JpegEncoder
                {
                    Quality = 80
                });

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The provided data is not a valid image.", ex);
            }
        }

        private bool IsValidImageFile(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return Array.Exists(_validExtensions, ext => ext.Equals(extension));
        }
    }
}