using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implementation
{
    public class PhotoUploadService : IPhotoUploadService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoUploadService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadPhotoAsync(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return null;
            }

            using var stream = photo.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(photo.FileName, stream),
                Transformation = new Transformation()
                    .Width(500).Height(500).Crop("fill").Gravity("face")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return uploadResult.SecureUrl.ToString();
        }
    }
}
