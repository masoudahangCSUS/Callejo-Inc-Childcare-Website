using System;

namespace BlazorApp.Server.Models
{
    public class PhoneNumber
    {
        public long Id { get; set; }
        public Guid FkUsers { get; set; }
        public string AreaCode { get; set; }
        public string Prefix { get; set; }
        public string LastFour { get; set; }
        public long FkType { get; set; }

        public CallejoIncUser User { get; set; }
        public PhoneNumberType PhoneNumberType { get; set; }
    }
}

