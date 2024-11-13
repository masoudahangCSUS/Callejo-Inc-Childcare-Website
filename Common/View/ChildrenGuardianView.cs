using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class ChildrenGuardianView
    {
        public long childId { get; set; }
        public string childFirstName { get; set; }
        public string childMiddleName { get; set; }
        public string childLastName { get; set; }
        public long guardianId { get; set; }
        public string guadianFirstName { get; set; }
        public string guardianMiddleName { get; set; }
        public string guardianLastName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
    }

    public class ListChildrenGuardianView : APIResponse
    {
        public List<ChildrenGuardianView> listChildrenGuardian { get; set; }
        public ListChildrenGuardianView()
        {
            listChildrenGuardian = new List<ChildrenGuardianView>();
        }
    }
}
