using Common.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Submit
{
    public class SubmitService : ISubmitService
    {
        // Construct service with DB Context for DB manipulation
        private readonly CallejoSystemDbContext _context;
        public SubmitService(CallejoSystemDbContext context) 
        {
            _context = context;
        }
        public async Task AddInquiryAsync(InterestedParent inquiry) 
        {
            _context.InterestedParents.Add(inquiry);
            await _context.SaveChangesAsync();
        }
        public async Task<List<InterestedParent>> GetInquiryAsync() 
        {
            return await _context.InterestedParents.ToListAsync();
        }
        public async void DeleteInquiryAsync() 
        { 
        }
    }
}
