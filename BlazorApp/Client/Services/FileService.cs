using Common.Models.Data;
using Common.Services.Role;
using System.Reflection.Metadata;

namespace BlazorApp.Client.Services
{
    public class FileService : IFileService
    {
        private readonly CallejoSystemDbContext _dbContext;

        public FileService(CallejoSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UploadFileAsync(Guid userId, byte[] fileData)
        {
            var user = await _dbContext.CallejoIncUsers.FindAsync(userId);
            if (user != null)
            {
                user.RegistrationDocument = fileData;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetFileAsync(Guid userId)
        {
            var user = await _dbContext.CallejoIncUsers.FindAsync(userId);
            return user?.RegistrationDocument;
        }
    }
}
