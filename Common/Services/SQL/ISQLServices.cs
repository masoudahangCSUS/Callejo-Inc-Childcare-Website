using Common.Models.Data;
using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.SQL
{
    public interface ISQLServices
    {
        ListChildrenGuardianView GetListOfAllChildrenAndGuardians();

        // New methods for Notifications
        IEnumerable<Notification> GetNotificationsByParentId(Guid parentId);
        bool MarkNotificationAsRead(long id);
        bool SendCustomNotification(string parentId, string message);

        // New method for Holidays & Vacations
        IEnumerable<HolidaysVacations> GetHolidaysVacations();

    }

}