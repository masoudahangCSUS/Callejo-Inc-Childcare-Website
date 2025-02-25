using Common.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Registration
{
    public class RegService : IRegService
    {
        private readonly CallejoSystemDbContext _dbContext;

        public RegService(CallejoSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UploadFileAsync(Guid userId, byte[] fileData, string fileType, long fileSize)
        {
            try
            {
                // Find user in DB. Return false if not found
                var user = await _dbContext.CallejoIncUsers.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                // Store file in registration_document column
                user.RegistrationDocument = fileData;

                // Save changes
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<byte[]?> GetFileAsync(Guid userId)
        {
            var user = await _dbContext.CallejoIncUsers.FindAsync(userId);
            return user?.RegistrationDocument;
        }

        public async Task<bool> DeleteFileAsync(Guid userId)
        {
            try
            {
                // Find user in DB. Return false if not found
                var user = await _dbContext.CallejoIncUsers.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                // Set registration document to null and save changes
                user.RegistrationDocument = null;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
