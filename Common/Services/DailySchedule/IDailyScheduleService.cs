using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.DailySchedule
{
    public interface IDailyScheduleService
    {
        APIResponse InsertDailySchedule(DailyScheduleView dailyScheduleView);
        ListDailySchedule GetDailyScheduleByDate(DateTime date);
    }
}
