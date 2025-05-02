using Common.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Services.Submit
{
    public class SubmitService : ISubmitService
    {
        private readonly CallejoSystemDbContext _context;

        public SubmitService(CallejoSystemDbContext context)
        {
            _context = context;
        }

        // Add a new inquiry
        public async Task AddInquiryAsync(InterestedParent inquiry)
        {
            if (inquiry == null)
                throw new ArgumentNullException(nameof(inquiry));

            // Ensure ID and Datetime are set
            inquiry.Id = inquiry.Id == Guid.Empty ? Guid.NewGuid() : inquiry.Id;
            inquiry.Datetime ??= DateTime.UtcNow;

            _context.InterestedParents.Add(inquiry);
            await _context.SaveChangesAsync();
        }

        // Get all inquiries, sorted by Datetime descending
        public async Task<List<InterestedParent>> GetInquiryAsync()
        {
            return await _context.InterestedParents
                                 .OrderByDescending(i => i.Datetime)
                                 .ToListAsync();
        }

        // Delete an inquiry by ID
        public async Task<bool> DeleteInquiryAsync(Guid id)
        {
            var inquiry = await _context.InterestedParents.FindAsync(id);
            if (inquiry == null)
                return false;

            _context.InterestedParents.Remove(inquiry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
