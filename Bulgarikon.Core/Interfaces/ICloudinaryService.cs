using Bulgarikon.Core.DTOs.Common;
using Microsoft.AspNetCore.Http;

namespace Bulgarikon.Core.Interfaces
{
    public interface ICloudinaryService
    {
        Task<CloudinaryUploadResultDto> UploadImageAsync(IFormFile file);
        Task DeleteImageAsync(string publicId);
    }
}
