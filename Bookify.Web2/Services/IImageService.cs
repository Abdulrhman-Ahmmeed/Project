namespace Bookify.Web2.Services
{
    public interface IImageService
    {
        Task<(bool IsUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string imageFolder, bool IsThumbnail);
        void Delete(string imagePath, string? imageThumbnailPath = null);
    }
}
