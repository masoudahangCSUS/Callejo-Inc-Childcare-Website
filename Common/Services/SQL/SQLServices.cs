using Common.Models.Data;
using Common.View;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.SQL
{
    public class SQLServices : ISQLServices
    {
        private CallejoSystemDbContext _context;

        public SQLServices(CallejoSystemDbContext context)
        {
            _context = context;
        }
        public ListChildrenGuardianView GetListOfAllChildrenAndGuardians()
        {
            ListChildrenGuardianView listChildren = new ListChildrenGuardianView();

            try
            {
                string connectionString = _context.Database.GetConnectionString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
               select
                  c.id as childId,
                  ISNULL(c.first_name, '') as childFirstName,
                  ISNULL(c.middle_name, '') as childMiddleName,
                  c.last_name as childLastName,
                  ISNULL(ciu.first_name, '') as guadianFirstName,
                  ISNULL(ciu.middle_name, '') as guardianMiddleName,
                  ciu.last_name as guardianLastName,
                  ciu.address,
                  ciu.city,
                  ciu.zip_code
               from
                  Guardians g
                  inner join Children c
                     on c.id = g.fk_children
                  inner join Callejo_Inc_Users ciu
                     on ciu.id = g.fk_parent
               order by
                  c.id
            ";

                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                    DataSet dataSet = new DataSet();
                    sqlAdapter.Fill(dataSet);

                    ChildrenGuardianView childrenGuadianView = null;
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        childrenGuadianView = new ChildrenGuardianView();
                        childrenGuadianView.childId = long.Parse(dataRow["childId"].ToString());
                        childrenGuadianView.childFirstName = dataRow["childFirstName"].ToString();
                        childrenGuadianView.childMiddleName = dataRow["childMiddleName"].ToString();
                        childrenGuadianView.childLastName = dataRow["childLastName"].ToString();
                        childrenGuadianView.guardianId = long.Parse(dataRow["guardianId"].ToString());
                        childrenGuadianView.guadianFirstName = dataRow["guadianFirstName"].ToString();
                        childrenGuadianView.guardianMiddleName = dataRow["guardianMiddleName"].ToString();
                        childrenGuadianView.guardianLastName = dataRow["guardianLastName"].ToString();
                        childrenGuadianView.address = dataRow["address"].ToString();
                        childrenGuadianView.city = dataRow["city"].ToString();
                        childrenGuadianView.zip_code = dataRow["zip_code"].ToString();

                        listChildren.listChildrenGuardian.Add(childrenGuadianView);
                    }

                    listChildren.Success = true;
                    listChildren.Message = "Retrieved " + listChildren.listChildrenGuardian.Count.ToString() + " number of records";
                }
            }
            catch (Exception ex)
            {
                listChildren.Success = false;
                listChildren.Message = "Problems retrieving children and guardian records. Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }
            return listChildren;
        }
        public IEnumerable<NotificationView> GetNotificationsByParentId(Guid parentId)
        {
            return _context.Notifications
                .Where(n => n.FkParentId == parentId)
                .OrderByDescending(n => n.SentOn)
                .Select(n => new NotificationView
                {
                    Id = n.Id,
                    FkParentId = n.FkParentId,
                    Title = n.Title,
                    Message = n.Message,
                    SentOn = n.SentOn,
                    IsRead = n.IsRead,
                    IsExpanded = n.IsExpanded
                })
                .ToList();
        }

        // New Method: Fetch a users Phone Number by their ID
        public async Task<IEnumerable<PhoneNumber>> GetPhoneNumber(Guid? userID, long type)
        {
            return await _context.PhoneNumbers
                .Where(n => n.FkUsers == userID)
                .Where(n => n.FkType == type)
                .ToListAsync();

        }

        public bool MarkNotificationAsRead(long id)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.Id == id);
            if (notification == null)
            {
                return false;
            }

            notification.IsRead = true;
            _context.SaveChanges();
            return true;
        }

        // New Method: Create custom notification from parent to owner
        public bool SendCustomNotification(NotificationView newRequest)
        {
            try
            {
                var newNotif = new Notification
                {
                    FkParentId = newRequest.FkParentId,
                    Title = newRequest.Title,
                    Message = newRequest.Message,
                    SentOn = DateTime.UtcNow,
                    IsRead = false,
                    IsExpanded = false
                };

                _context.Notifications.Add(newNotif);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving notification: {ex.Message}");
                return false;
            }
        }
        // Create said notification
        /*var newNotif = new Notification
        {

            FkParentId = Guid.Parse("F7DE2748-4FB0-4A78-8EF7-014C4D716A9B"),    // Hardcoded owner GUID -- change later
            Title = "CUSTOM NOTIFICATION FROM: " + newRequest.Title,
            Message = newRequest.Message,
            SentOn = DateTime.Now,
            IsRead = false,
        };

        // Add notification to db, save, and return
        _context.Notifications.Add(newNotif);
        _context.SaveChanges();
        return true;*/
        //  }


        // New Method: Fetch all holidays & vacations
        public IEnumerable<HolidaysVacationView> GetHolidaysVacations()
        {
            return _context.HolidaysVacations
                .OrderBy(h => h.StartDate)
                .Select(h => new HolidaysVacationView
                {
                    Id = h.Id,
                    Title = h.Title,
                    Description = h.Description,
                    StartDate = h.StartDate,
                    EndDate = h.EndDate,
                    Type = h.Type,
                    CreatedAt = h.CreatedAt ?? DateTime.UtcNow
                })
                .ToList();
        }

        // Create a new holiday/vacation (Admin)
        public bool CreateHolidayVacation(HolidaysVacationView holidayVacationView)
        {
            if (holidayVacationView == null)
                return false;

            try
            {
                var holidayVacation = new HolidaysVacation
                {
                    Title = holidayVacationView.Title,
                    Description = holidayVacationView.Description,
                    StartDate = holidayVacationView.StartDate,
                    EndDate = holidayVacationView.EndDate,
                    Type = holidayVacationView.Type,
                    CreatedAt = holidayVacationView.CreatedAt ?? DateTime.UtcNow
                };

                _context.HolidaysVacations.Add(holidayVacation);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating holiday/vacation: {ex.Message}");
                return false;
            }
        }


        // Update an existing holiday/vacation (Admin)
        public bool UpdateHolidayVacation(long id, HolidaysVacationView updatedHolidayVacation)
        {
            var existingHoliday = _context.HolidaysVacations.FirstOrDefault(h => h.Id == id);

            if (existingHoliday == null)
                return false;

            try
            {
                existingHoliday.Title = updatedHolidayVacation.Title;
                existingHoliday.Description = updatedHolidayVacation.Description;
                existingHoliday.StartDate = updatedHolidayVacation.StartDate;
                existingHoliday.EndDate = updatedHolidayVacation.EndDate;
                existingHoliday.Type = updatedHolidayVacation.Type;
                existingHoliday.CreatedAt = updatedHolidayVacation.CreatedAt ?? existingHoliday.CreatedAt;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating holiday/vacation: {ex.Message}");
                return false;
            }
        }

        //Delete a holiday/vacation(Admin)
        public bool DeleteHolidayVacation(long id)
        {
            var holidayVacation = _context.HolidaysVacations.FirstOrDefault(h => h.Id == id);

            if (holidayVacation == null)
                return false;

            try
            {
                _context.HolidaysVacations.Remove(holidayVacation);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting holiday/vacation: {ex.Message}");
                return false;
            }
        }


        public bool CreateNotification(NotificationView notification)
        {
            try
            {
                var newNotif = new Notification
                {
                    FkParentId = notification.FkParentId,
                    Title = notification.Title,
                    Message = notification.Message,
                    SentOn = DateTime.UtcNow,
                    IsRead = false,
                    IsExpanded = false
                };

                _context.Notifications.Add(newNotif);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating notification: {ex.Message}");
                return false;
            }
        }


        public bool UpdateNotification(long id, NotificationView updatedNotification)
        {
            try
            {
                var notification = _context.Notifications.FirstOrDefault(n => n.Id == id);
                if (notification == null) return false;

                notification.Title = updatedNotification.Title;
                notification.Message = updatedNotification.Message;
                notification.SentOn = updatedNotification.SentOn;
                notification.IsRead = updatedNotification.IsRead;
                notification.IsExpanded = updatedNotification.IsExpanded;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating notification: {ex.Message}");
                return false;
            }
        }

        public bool DeleteNotification(long id)
        {
            try
            {
                var notification = _context.Notifications.FirstOrDefault(n => n.Id == id);
                if (notification == null) return false;

                _context.Notifications.Remove(notification);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting notification: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<NotificationView> GetAllNotifications()
        {
            return _context.Notifications
                .OrderByDescending(n => n.SentOn)
                .Select(n => new NotificationView
                {
                    Id = n.Id,
                    FkParentId = n.FkParentId,
                    Title = n.Title,
                    Message = n.Message,
                    SentOn = n.SentOn,
                    IsRead = n.IsRead,
                    IsExpanded = n.IsExpanded
                })
                .ToList();
        }
        
        
        public async Task<IEnumerable<long>> GetChildren(Guid? id)
        {
            var user = await _context.CallejoIncUsers
            .Include(u => u.FkChildren)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return Enumerable.Empty<long>();
            }


            return user.FkChildren.Select(child => child.Id);
        }
        
        

        public async Task<Child> getChildById(long id)
        {
            return await _context.Children
                           .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<CallejoIncUser?> getUserWithNumber(Guid id)
        {
            return await _context.CallejoIncUsers
                        .Include(u => u.PhoneNumbers)
                        .FirstOrDefaultAsync(u => u.Id == id); ;
        }

        public async Task<bool> updateUser(CallejoIncUser user, CustomerUserViewDTO userDto)
        {
            if (user == null)
            {
                return false;
            }


            // Update basic user fields.
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Address = userDto.Address;
            user.City = userDto.City;
            user.State = userDto.State;
            user.ZipCode = userDto.ZipCode;

            // Update primary phone (type 1) – assuming primary is required.
            if (userDto.PrimaryPhoneNumber != null)
            {
                var primaryPhone = user.PhoneNumbers.FirstOrDefault(p => p.FkType == 1);
                if (primaryPhone != null)
                {
                    primaryPhone.AreaCode = userDto.PrimaryPhoneNumber.AreaCode;
                    primaryPhone.Prefix = userDto.PrimaryPhoneNumber.Prefix;
                    primaryPhone.LastFour = userDto.PrimaryPhoneNumber.LastFour;
                }
                else
                {
                    var newPrimaryPhone = new PhoneNumber
                    {
                        FkUsers = user.Id,
                        FkType = 1,
                        AreaCode = userDto.PrimaryPhoneNumber.AreaCode,
                        Prefix = userDto.PrimaryPhoneNumber.Prefix,
                        LastFour = userDto.PrimaryPhoneNumber.LastFour
                    };
                    _context.PhoneNumbers.Add(newPrimaryPhone);
                }
            }

            // Update secondary phone (type 2) only if the DTO field is not null.
            if (userDto.SecondaryPhoneNumber != null)
            {
                var secondaryPhone = user.PhoneNumbers.FirstOrDefault(p => p.FkType == 2);
                if (secondaryPhone != null)
                {
                    secondaryPhone.AreaCode = userDto.SecondaryPhoneNumber.AreaCode;
                    secondaryPhone.Prefix = userDto.SecondaryPhoneNumber.Prefix;
                    secondaryPhone.LastFour = userDto.SecondaryPhoneNumber.LastFour;
                }
                else
                {
                    var newSecondaryPhone = new PhoneNumber
                    {
                        FkUsers = user.Id,
                        FkType = 2,
                        AreaCode = userDto.SecondaryPhoneNumber.AreaCode,
                        Prefix = userDto.SecondaryPhoneNumber.Prefix,
                        LastFour = userDto.SecondaryPhoneNumber.LastFour
                    };
                    _context.PhoneNumbers.Add(newSecondaryPhone);
                }
            }
            // If SecondaryPhoneNumber is null, no action is taken for that field.

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"updateUser exception: {ex}");
                return false;
            }
        }

        public async Task<bool> updateEmergencyContact(EmergencyContact emergencyContact, EmergencyContactDTO emergencyDto)
        {
            emergencyContact.FirstName = emergencyDto.FirstName;
            emergencyContact.LastName = emergencyDto.LastName;
            emergencyContact.Relationship = emergencyDto.Relationship;

            // Update emergency primary phone (type 3).
            if (emergencyDto.PrimaryPhoneNumber != null)
            {
                var emergencyPrimary = await _context.PhoneNumbers
                                            .FirstOrDefaultAsync(p => p.FkUsers == emergencyContact.FkUsers && p.FkType == 3);
                if (emergencyPrimary != null)
                {
                    emergencyPrimary.AreaCode = emergencyDto.PrimaryPhoneNumber.AreaCode;
                    emergencyPrimary.Prefix = emergencyDto.PrimaryPhoneNumber.Prefix;
                    emergencyPrimary.LastFour = emergencyDto.PrimaryPhoneNumber.LastFour;
                }
                else
                {
                    var newEmergencyPrimary = new PhoneNumber
                    {
                        FkUsers = emergencyContact.FkUsers,
                        FkType = 3,
                        AreaCode = emergencyDto.PrimaryPhoneNumber.AreaCode,
                        Prefix = emergencyDto.PrimaryPhoneNumber.Prefix,
                        LastFour = emergencyDto.PrimaryPhoneNumber.LastFour
                    };
                    _context.PhoneNumbers.Add(newEmergencyPrimary);
                }
            }

            // Update emergency secondary phone (type 4) only if the DTO field is not null.
            if (emergencyDto.SecondaryPhoneNumber != null)
            {
                var emergencySecondary = await _context.PhoneNumbers
                                            .FirstOrDefaultAsync(p => p.FkUsers == emergencyContact.FkUsers && p.FkType == 4);
                if (emergencySecondary != null)
                {
                    emergencySecondary.AreaCode = emergencyDto.SecondaryPhoneNumber.AreaCode;
                    emergencySecondary.Prefix = emergencyDto.SecondaryPhoneNumber.Prefix;
                    emergencySecondary.LastFour = emergencyDto.SecondaryPhoneNumber.LastFour;
                }
                else
                {
                    var newEmergencySecondary = new PhoneNumber
                    {
                        FkUsers = emergencyContact.FkUsers,
                        FkType = 4,
                        AreaCode = emergencyDto.SecondaryPhoneNumber.AreaCode,
                        Prefix = emergencyDto.SecondaryPhoneNumber.Prefix,
                        LastFour = emergencyDto.SecondaryPhoneNumber.LastFour
                    };
                    _context.PhoneNumbers.Add(newEmergencySecondary);
                }
            }
            // If SecondaryPhoneNumber is null in emergencyDto, then no update is performed.

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Optionally log the exception.
                return false;
            }
        }

        public async Task<bool> updateChild(Child child, ChildDTO childDTO)
        {
            if (child == null || childDTO == null)
                return false;

            try
            {
                // Update the child.
                child.FirstName = childDTO.FirstName;
                child.MiddleName = childDTO.MiddleName;
                child.LastName = childDTO.LastName;
                child.Age = childDTO.Age;

                // Mark the entity as modified.
                _context.Children.Update(child);

                // Save the changes to the database.
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       
        public async Task<bool> updatePassowrd(SettingsDTO settings)
        {
            // Retrieve the user by Id from the database.
            var user = await _context.CallejoIncUsers.FindAsync(settings.Id);
            if (user == null)
            {
                return false;
            }

            // Update the password field using the value from SettingsDTO.
            user.Password = settings.Password;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Consider logging the exception in a real-world scenario.
                return false;
            }
        }

        public async Task<bool> updateEmail(SettingsDTO settings)
        {

            // Retrieve the user by Id from the database.
            var user = await _context.CallejoIncUsers.FindAsync(settings.Id);
            if (user == null)
            {
                return false;
            }

            // Update the email field using the value from SettingsDTO.
            user.Email = settings.Email;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Consider logging the exception in a real-world scenario.
                return false;
            }
        }

    }

}