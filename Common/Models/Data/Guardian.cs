using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Data
{
    public class Guardian
    {
        public Guid? fk_parent { get; set; }
        public long fk_child { get; set; }
    }
}
