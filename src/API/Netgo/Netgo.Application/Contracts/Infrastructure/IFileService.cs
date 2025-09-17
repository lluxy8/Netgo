using Microsoft.AspNetCore.Http;

namespace Netgo.Application.Contracts.Infrastructure
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(string folderName, IFormFile file);
        Task<IEnumerable<string>> GetFilesAsync(string folderName);
        Task DeleteFileAsync(string folderName, string fileName);
        Task DeleteFolderAsync(string folderName);
    }
}
