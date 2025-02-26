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
        // New Method: Fetch notifications by parent ID
        public IEnumerable<Notification> GetNotificationsByParentId(Guid parentId)
        {
            return _context.Notifications
                .Where(n => n.FkParentId == parentId)
                .OrderByDescending(n => n.SentOn)
                .ToList();
        }

        // New Method: Mark a notification as read
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
        public bool SendCustomNotification(Notification newRequest)
        {
            try
            {
                string query = @"INSERT INTO Notifications (FkParentId, Title, Message, SentOn, IsRead) VALUES (@FkParentId, @Title, @Message, @SentOn, @IsRead)";

                var parameters = new
                {
                    FkParentId = newRequest.FkParentId,
                    Title = newRequest.Title,
                    Message = newRequest.Message,
                    SentOn = newRequest.SentOn,
                    IsRead = newRequest.IsRead
                };

                int rowsAffected = _context.Database.ExecuteSqlRaw(query,
                    new SqlParameter("@FkParentId", newRequest.FkParentId),
                    new SqlParameter("@Title", newRequest.Title),
                    new SqlParameter("@Message", newRequest.Message),
                    new SqlParameter("@SentOn", newRequest.SentOn),
                    new SqlParameter("@IsRead", newRequest.IsRead)
                );
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving notification: {ex.Message}");
                return false;
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
        }

        // New Method: Fetch all holidays & vacations
        public IEnumerable<HolidaysVacations> GetHolidaysVacations()
        {
            return _context.HolidaysVacations
                .OrderBy(h => h.StartDate)
                .ToList();
        }

        public bool CreateNotification(Notification notification)
        {
            if (notification == null)
            {
                return false;
            }

            // Ensure IsRead is always false by default
            notification.IsRead = false;

            try
            {
                _context.Notifications.Add(notification);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating notification: {ex.Message}");
                return false;
            }
        }


        public bool UpdateNotification(long id, Notification updatedNotification)
        {
            try
            {
                using var dbContext = new CallejoSystemDbContext();
                var notification = dbContext.Notifications.FirstOrDefault(n => n.Id == id);

                if (notification == null) return false;

                notification.Title = updatedNotification.Title;
                notification.Message = updatedNotification.Message;
                notification.SentOn = updatedNotification.SentOn;
                notification.IsRead = updatedNotification.IsRead;

                dbContext.SaveChanges();
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
                using var dbContext = new CallejoSystemDbContext();
                var notification = dbContext.Notifications.FirstOrDefault(n => n.Id == id);

                if (notification == null) return false;

                dbContext.Notifications.Remove(notification);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting notification: {ex.Message}");
                return false;
            }
        }

        // Fetch all notifications for admin
        public IEnumerable<Notification> GetAllNotifications()
        {
            return _context.Notifications
                .OrderByDescending(n => n.SentOn)
                .ToList();
        }


    }

}