using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services.Interface;

public interface IFileService
{
    public Task<string?> UploadFileAsync(IFormFile file);
    public Task<(bool Success, string Message)> DeleteImageByUrlAsync(string? imageUrl);
}