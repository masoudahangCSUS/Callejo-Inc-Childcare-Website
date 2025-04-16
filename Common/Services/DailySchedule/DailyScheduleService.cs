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

                dailySchedule.CreatedAt = dailyScheduleView.CreatedAt;
                dailySchedule.Description = dailyScheduleView.Description;
                if (!string.IsNullOrEmpty(dailyScheduleView.Desc_special))
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

        public ListDailySchedule GetDailyScheduleByDate(DateOnly date)
        {
            ListDailySchedule listDailySchedule = new ListDailySchedule();

            try
            {
                var dailyScheduleRec = _context.DailySchedules.Where(r => r.CreatedAt == date).FirstOrDefault();

                if (dailyScheduleRec != null)
                {
                    DailyScheduleView dailyScheduleViewRec = new DailyScheduleView();
                    dailyScheduleViewRec.Id = dailyScheduleRec.Id;
                    dailyScheduleViewRec.Description = dailyScheduleRec.Description;
                    dailyScheduleViewRec.Desc_special = dailyScheduleRec.DescSpecial;
                    dailyScheduleViewRec.CreatedAt = dailyScheduleRec.CreatedAt;

                    listDailySchedule.dailySchedules.Add(dailyScheduleViewRec);
                    listDailySchedule.Message = "Retrieved daily schedule record that matched date " + date.ToString();
                }
                else
                {
                    listDailySchedule.Status = "No record matching id " + date.ToString() + " was found";
                }
                listDailySchedule.Success = true;
            }
            catch (Exception ex)
            {
                listDailySchedule.Success = false;
                listDailySchedule.Message = "Problems retrieve daily schedule record " + date.ToString() + ". Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return listDailySchedule;
        }


        /// <summary>
        /// Retrieve a daily schedule record
        /// </summary>
        /// <param name="id">Primary key for daily schedule record</param>
        /// <returns></returns>
        public ListDailySchedule GetDailyScheduleById(long id)
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
                    dailyScheduleViewRec.Desc_special = dailyScheduleRec.DescSpecial;
                    dailyScheduleViewRec.CreatedAt = dailyScheduleRec.CreatedAt;

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

        /// <summary>
        /// Will update daily schedule record
        /// </summary>
        /// <param name="dailyScheduleView">Contains id of record to update and description</param>
        /// <returns></returns>
        public APIResponse UpdateDailySchedule(DailyScheduleView dailyScheduleView)
        {
            APIResponse response = new APIResponse();

            try
            {
                var dailyScheduleRec = _context.Roles.Where(r => r.Id == dailyScheduleView.Id).FirstOrDefault();

                if (dailyScheduleRec != null)
                {
                    dailyScheduleRec.Description = dailyScheduleView.Description;
                    _context.SaveChanges();
                    response.Message = "Daily schedule record with id " + dailyScheduleView.Id.ToString() + " was updated to database";
                }
                else
                {
                    response.Message = "No Daily schedule record with id " + dailyScheduleView.Id.ToString() + " was found in database";
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Problems updating daily schedule record " + dailyScheduleView.Id + ". Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return response;
        }

        /// <summary>
        /// Deletes a daily schedule record in daoly schedule database
        /// </summary>
        /// <param name="dailyScheduleRec">Data to be deleted. Since the id field is an auto increment field we will not retrieve that value</param>
        /// <returns></returns>
        public APIResponse DeleteDailySchedule(long id)
        {
            APIResponse response = new APIResponse();

            try
            {
                var dailyScheduleRec = _context.DailySchedules.Where(r => r.Id == id).FirstOrDefault();

                if (dailyScheduleRec != null)
                {
                    response.Message = "Daily Schuledule record " + dailyScheduleRec.Id.ToString() + " on the date " + dailyScheduleRec.CreatedAt + " has been deleted";
                    _context.DailySchedules.Remove(dailyScheduleRec);
                    _context.SaveChanges();
                }
                else
                {
                    response.Message = "No record with the id of " + id.ToString() + " was found";
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Problems deleting daily schuledule record. Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return response;
        }
        public ListDailySchedule GetAllDailySchedules()
        {
            ListDailySchedule listDailySchedule = new();

            try
            {
                var dailyScheduleRecs = _context.DailySchedules.ToList();
                DailyScheduleView dailyScheduleViewRec = null;
                foreach (Models.Data.DailySchedule dailyScheduleRec in dailyScheduleRecs)
                {
                    dailyScheduleViewRec = new DailyScheduleView();
                    dailyScheduleViewRec.Id = dailyScheduleRec.Id;
                    dailyScheduleViewRec.Description = dailyScheduleRec.Description;
                    dailyScheduleViewRec.Desc_special = dailyScheduleRec.DescSpecial;
                    dailyScheduleViewRec.CreatedAt = dailyScheduleRec.CreatedAt;

                    listDailySchedule.dailySchedules.Add(dailyScheduleViewRec);
                }

                listDailySchedule.Success = true;
                listDailySchedule.Message = "Retrieved " + listDailySchedule.dailySchedules.Count.ToString() + " schedule records";
            }
            catch (Exception ex)
            {
                listDailySchedule.Success = false;
                listDailySchedule.Message = "Problems retrieving all daily schedule records. Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return listDailySchedule;
        }
    }
}