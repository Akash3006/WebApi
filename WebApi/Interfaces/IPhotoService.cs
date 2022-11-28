using CloudinaryDotNet.Actions;

namespace WebApi.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsycn(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}