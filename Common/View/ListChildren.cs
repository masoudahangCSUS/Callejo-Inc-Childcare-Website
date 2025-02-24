using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class ListChildren
    {
        public List<ChildView> children { get; set; } = new List<ChildView>();
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}