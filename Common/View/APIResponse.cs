using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class APIResponse
    {
        public APIResponse()
        {
            Success = true;
        }
        public bool Success { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public object Token { get; set; }

    }

}
