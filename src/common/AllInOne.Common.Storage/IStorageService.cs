using System;
using System.IO;
using System.Threading.Tasks;

namespace AllInOne.Common.Storage
{
    public interface IStorageService
    {
        Task CreateIfNotExistsAsync();
        Task<Uri> SaveFileAsync(Stream stream, string fileName);
        Task RemoveFileAsync(string fileName);
        Task DeleteAsync();
    }
}
