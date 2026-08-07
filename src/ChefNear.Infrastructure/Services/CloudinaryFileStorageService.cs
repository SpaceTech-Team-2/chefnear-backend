using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Infrastructure.Services
{
  
    public class CloudinaryFileStorageService : IFileStorageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _settings;

        public CloudinaryFileStorageService(IOptions<CloudinarySettings> options)
        {
            _settings = options.Value;

            var account = new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = _settings.DishImagesFolder,
                // Cloudinary auto-optimizes format/quality for the requesting client.
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (result.Error is not null)
            {
                throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");
            }

            return result.SecureUrl.ToString();
        }

        public async Task DeleteImageAsync(string imageUrl, CancellationToken cancellationToken = default)
        {
            var publicId = ExtractPublicIdFromUrl(imageUrl);
            if (string.IsNullOrEmpty(publicId))
            {
                return;
            }

            var deleteParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }

      
        private static string? ExtractPublicIdFromUrl(string imageUrl)
        {
            var uploadMarker = "/upload/";
            var uploadIndex = imageUrl.IndexOf(uploadMarker, StringComparison.OrdinalIgnoreCase);
            if (uploadIndex < 0)
            {
                return null;
            }

            var afterUpload = imageUrl[(uploadIndex + uploadMarker.Length)..];

            var segments = afterUpload.Split('/');
            var startIndex = segments.Length > 0 && segments[0].StartsWith('v') && segments[0].Length > 1 &&
                              segments[0][1..].All(char.IsDigit)
                ? 1
                : 0;

            var pathWithExtension = string.Join('/', segments.Skip(startIndex));
            var lastDot = pathWithExtension.LastIndexOf('.');

            return lastDot > 0 ? pathWithExtension[..lastDot] : pathWithExtension;
        }
    }

}
