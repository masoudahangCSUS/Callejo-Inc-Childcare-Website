using Common.Models.Data;
using Common.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Creates a role reccord in role database
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
    }
}
