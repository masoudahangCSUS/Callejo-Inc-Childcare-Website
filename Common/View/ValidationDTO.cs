using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class ValidationDTO
    {
        public Guid UserId { get; set; }
        public string TotpCode { get; set; }
    }
}
