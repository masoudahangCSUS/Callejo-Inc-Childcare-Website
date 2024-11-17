using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class AdminUserUpdateDTO
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public long FkRole { get; set; }
        public string? PhoneNumber { get; set; }

        public List<ChildView>? ChildrenToAdd { get; set; } = new List<ChildView>();
        public List<long>? ChildrenToRemove { get; set; } = new();
        public List<ChildView>? ChildrenToUpdate { get; set; }
    }
}
