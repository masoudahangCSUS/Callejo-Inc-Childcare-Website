using System.Collections.Generic;

namespace BlazorApp.Models
{
    public class Child
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public ICollection<Guardian> Guardians { get; set; }
    }
}
