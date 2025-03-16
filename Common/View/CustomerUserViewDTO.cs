using Common.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class CustomerUserViewDTO
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public PhoneNumberDTO? PrimaryPhoneNumber { get; set; }
        public PhoneNumberDTO? SecondaryPhoneNumber { get; set; }

        public List<ChildView> Children { get; set; } = new List<ChildView>();

    }
}