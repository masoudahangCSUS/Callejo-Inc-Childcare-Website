using Common.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class EmergencyContactDTO
    {
        public string FirstName { get; set;}
        public string LastName { get; set; }
        public string Relationship { get; set; }
        public PhoneNumber? PrimaryPhoneNumber { get; set; }
        public PhoneNumber? SecondaryPhoneNumber { get; set; }
    }
}
