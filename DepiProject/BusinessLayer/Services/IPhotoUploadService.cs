using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IPhotoUploadService
    {
        Task<string> UploadPhotoAsync(IFormFile photo);
    }
}
