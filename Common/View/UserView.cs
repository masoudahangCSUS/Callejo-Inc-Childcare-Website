using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class UserView
    {
        public Guid userId { get; set; }
        public string userFirstName { get; set; }
        public string userMiddleName { get; set; }
        public string userLastName { get; set; }
        public string userAddress { get; set; }
        public string userCity { get; set; }
        public string userState { get; set; }
        public string userZipCode { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public long userFkRole { get; set; }
        public string userPhoneNumber { get; set; }
    }
    public class ListUsers : APIResponse
    {
        public List<UserView> users { get; set; }

        public ListUsers()
        {
            users = new List<UserView>();
        }
    }
}