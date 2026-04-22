using Bulgarikon.Core.DTOs.Common;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Bulgarikon.Core.Implementations
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["CloudinarySettings:CloudName"];
            var apiKey = configuration["CloudinarySettings:ApiKey"];
            var apiSecret = configuration["CloudinarySettings:ApiSecret"];

            if (string.IsNullOrWhiteSpace(cloudName) ||
                string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(apiSecret))
            {
                throw new InvalidOperationException("Cloudinary settings are missing.");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            cloudinary = new Cloudinary(account);
        }

        public async Task DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return;

            var deleteParams = new DeletionParams(publicId);
            await cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<CloudinaryUploadResultDto> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "bulgarikon"
            };

            var result = await cloudinary.UploadAsync(uploadParams);

            return new CloudinaryUploadResultDto
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }

        public async Task<CloudinaryUploadResultDto> UploadImageFromUrlAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Invalid image URL");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageUrl),
                Folder = "bulgarikon"
            };

            var result = await cloudinary.UploadAsync(uploadParams);

            return new CloudinaryUploadResultDto
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }
    }
}