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

        // Parent Notifications
        IEnumerable<Notification> GetNotificationsByParentId(Guid parentId);
        bool MarkNotificationAsRead(long id);
        bool SendCustomNotification(Notification notification);

        // Admin Notifications
        IEnumerable<Notification> GetAllNotifications();  // <-- New method
        bool CreateNotification(Notification notification);
        bool UpdateNotification(long id, Notification updatedNotification);
        bool DeleteNotification(long id);

        // Holidays & Vacations
        IEnumerable<HolidaysVacations> GetHolidaysVacations();
    }



}