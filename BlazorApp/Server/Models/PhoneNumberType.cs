using System.Collections.Generic;

namespace BlazorApp.Server.Models
{
    public class PhoneNumberType
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
