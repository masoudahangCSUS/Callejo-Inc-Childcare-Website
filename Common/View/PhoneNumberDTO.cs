using System;

namespace Common.View
{
    public class PhoneNumberDTO
    {   
        public long Id { get; set; }
        public string AreaCode { get; set; }
        public string Prefix { get; set; }
        public string LastFour { get; set; }
        public Guid Fk_users { get; set; }
        public long Type { get; set; }
    }

    //public class ListPhoneNumbers : APIResponse
    //{
    //    public List<PhoneNumberDTO> phoneNumbers { get; set; }

    //    public ListPhoneNumbers()
    //    {
    //        phoneNumbers = new List<PhoneNumberDTO>();
    //    }
    //}
}
