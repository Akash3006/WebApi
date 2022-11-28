using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using WebApi.Helpers;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            //Cloudinary cloud credentials 
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecrets
            );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsycn(IFormFile file)
        {
            //Create upload result object
            var uploadResult = new ImageUploadResult();

            //Check file length
            if(file.Length > 0){
                //access the file stream to upload on cloudinary 
                using var str = file.OpenReadStream();

                //Add parameters 
                var uploadParams = new ImageUploadParams{
                    //File name
                    File = new FileDescription(file.FileName, str),

                    //Transformation
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),

                    //Folder 
                    Folder = "Images" 
                };

                //Upload and fetch the result 
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            //Create parameters object
            var deleteParams = new DeletionParams(publicId);

            //Delete the resource using parameters
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}