namespace Bookify.Web2.Services
{
    public class ImageService : IImageService
    {

        private readonly IWebHostEnvironment _environment;
        private readonly List<string> _AllowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private readonly int  _MaxAllowedSize = 2097152;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;

        }

      
        public async Task<(bool IsUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string imageFolder, bool IsThumbnail)
        {

            var extension = Path.GetExtension(image.FileName);
            if (!_AllowedExtensions.Contains(extension))
                return (IsUploaded:false, errorMessage: Errors.NotAllowedExtension);
            
            if (image.Length > _MaxAllowedSize)
                return (IsUploaded: false, errorMessage: Errors.MaxSize);

            string filePath = Path.Combine($"{_environment.WebRootPath}{imageFolder}", imageName);

            using var stream = File.Create(filePath);
            await image.CopyToAsync(stream);
            stream.Dispose();
            if (IsThumbnail)
            {
                string thumbPath = Path.Combine($"{_environment.WebRootPath}{imageFolder}/thumb", imageName);

                using var LoadedImage = Image.Load(image.OpenReadStream());
                float ratio = LoadedImage.Width / 200;

                LoadedImage.Mutate(i => i.Resize(width: 200, height: 300 / (int)ratio));
                LoadedImage.Save(thumbPath);
            }
            return (IsUploaded: true, errorMessage: null);


        }
        public void Delete(string imagePath, string? imageThumbnailPath = null)
        {
            var oldPath = $"{_environment.WebRootPath}{ imagePath}";

            //checking existing inside wwwroot
            if (File.Exists(oldPath))
            {
                //delete old picture using old path
               File.Delete(oldPath);
            }
            if (!string.IsNullOrEmpty(imageThumbnailPath))
            {
                var oldThumPath = $"{_environment.WebRootPath}{imageThumbnailPath}";
                if (File.Exists(oldThumPath))
                {
                    //delete old picture using old path
                    File.Delete(oldThumPath);
                }
            }
            
        }

    }
}
