using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Interfaces
{
    public interface IFileStorageService
    {
        
        Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);

        
        Task DeleteImageAsync(string imageUrl, CancellationToken cancellationToken = default);
    }
}
