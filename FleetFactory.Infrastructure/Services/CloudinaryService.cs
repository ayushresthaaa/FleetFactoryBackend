using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FleetFactory.Infrastructure.Services
{
    public class CloudinaryImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<(string ImageUrl, string PublicId)> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No image file provided");

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "fleetfactory/parts"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception(result.Error.Message);

            return (result.SecureUrl.ToString(), result.PublicId);
        }

        public async Task DeleteAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return;

            var deleteParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}