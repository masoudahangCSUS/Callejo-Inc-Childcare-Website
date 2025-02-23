using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Registration
{
    public interface IRegService
    {
        Task<bool> UploadFileAsync(Guid userId, byte[] fileData, string fileType, long fileSize);
        Task<byte[]?> GetFileAsync(Guid userId);
        Task<bool> DeleteFileAsync(Guid userId);
    }
}
