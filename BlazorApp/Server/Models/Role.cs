using System.Collections.Generic;

namespace BlazorApp.Server.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public ICollection<CallejoIncUser> Users { get; set; }
    }
}
