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
        IEnumerable<NotificationView> GetNotificationsByParentId(Guid parentId);
        bool MarkNotificationAsRead(long id);
        bool SendCustomNotification(NotificationView notification);

        // Admin Notifications
        IEnumerable<NotificationView> GetAllNotifications();
        bool CreateNotification(NotificationView notification);
        bool UpdateNotification(long id, NotificationView updatedNotification);
        bool DeleteNotification(long id);
        
        // Holidays & Vacations
        IEnumerable<HolidaysVacationView> GetHolidaysVacations();

        // Admin Holidays & Vacations
        bool CreateHolidayVacation(HolidaysVacationView holidayVacation);
        bool UpdateHolidayVacation(long id, HolidaysVacationView updatedHolidayVacation);
        bool DeleteHolidayVacation(long id);


        // New method for retrieving phone numbers
        public Task<IEnumerable<PhoneNumber>> GetPhoneNumber(Guid? ID, long type);
        public Task<Child> getChildById(long id);

        public Task<IEnumerable<long>> GetChildren(Guid? id);
        public Task<CallejoIncUser?> getUserWithNumber(Guid id);


        public Task<bool> updateUser(CallejoIncUser user, CustomerUserViewDTO userDto);

        public Task<bool> updateEmergencyContact(EmergencyContact emergencyContact, EmergencyContactDTO emergencyDto);
        public Task<bool> updateChild(Child childId, ChildDTO childDto);



    }



}