using Common.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Submit
{
    public interface ISubmitService
    {
        // Add Inquiries
        public Task AddInquiryAsync(InterestedParent inquiry);
        // Get Inquiries
        public Task<List<InterestedParent>> GetInquiryAsync();
        // Delete Inquiries
        public void DeleteInquiryAsync();


    }
}
