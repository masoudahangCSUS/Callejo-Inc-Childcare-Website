using Common.Models.Data;
using Common.View;

namespace Common.Services.DailySchedule
{
    public class DailyScheduleService : IDailyScheduleService
    {
        // DbContext class provides access to database
        private CallejoSystemDbContext _context;

        // Constructor uses dependency injection
        public DailyScheduleService(CallejoSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a daily schedule reccord in daily schedule database
        /// </summary>
        /// <param name="dailyScheduleView">Data to be saved.  Since the id field is an auto increment field we will not retrieve that value</param>
        /// <returns></returns>
        public APIResponse InsertDailySchedule(DailyScheduleView dailyScheduleView)
        {
            APIResponse response = new APIResponse();

            try
            {
                Models.Data.DailySchedule dailySchedule = new Models.Data.DailySchedule();
                dailySchedule.Day = dailyScheduleView.Day;
                dailySchedule.Month = dailyScheduleView.Month;
                dailySchedule.Year = dailyScheduleView.Year;
                dailySchedule.Description = dailyScheduleView.Description;
                if (dailyScheduleView.Description != null)
                {
                    dailySchedule.DescSpecial = dailyScheduleView.Desc_special;
                }

                _context.DailySchedules.Add(dailySchedule);
                _context.SaveChanges();

                // Retrieve the newly generated ID
                response.Data = dailySchedule.Id; // Access the ID here

                response.Message = "Daily Schedule record with description " + dailyScheduleView.Description + " was saved to database";

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Problems saving Daily Schedule record " + dailyScheduleView.Description + ". Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return response;
        }

        /// <summary>
        /// Retrieve a daily schedule record
        /// </summary>
        /// <param name="id">Primary key for daily schedule record</param>
        /// <returns></returns>
        public ListDailySchedule GetDailySchedule(long id)
        {
            ListDailySchedule listDailySchedule = new ListDailySchedule();

            try
            {
                var dailyScheduleRec = _context.DailySchedules.Where(r => r.Id == id).FirstOrDefault();

                if (dailyScheduleRec != null)
                {
                    DailyScheduleView dailyScheduleViewRec = new DailyScheduleView();
                    dailyScheduleViewRec.Id = dailyScheduleRec.Id;
                    dailyScheduleViewRec.Description = dailyScheduleRec.Description;

                    listDailySchedule.dailySchedules.Add(dailyScheduleViewRec);
                    listDailySchedule.Message = "Retrieved daily schedule record that matched id " + id.ToString();
                }
                else
                {
                    listDailySchedule.Status = "No record matching id " + id.ToString() + " would found";
                }
                listDailySchedule.Success = true;
            }
            catch (Exception ex)
            {
                listDailySchedule.Success = false;
                listDailySchedule.Message = "Problems retrieve daily schedule record " + id.ToString() + ". Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return listDailySchedule;
        }

    }
}
