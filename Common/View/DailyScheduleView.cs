namespace Common.View
{
    public class DailyScheduleView
    {
        public long Id { get; set; }
        public short Day { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public string Description { get; set; }
        public string  Desc_special { get; set; }
    }
    public class ListDailySchedule : APIResponse
    {
        public List<DailyScheduleView> dailySchedules { get; set; }

        public ListDailySchedule()
        {
            dailySchedules = new List<DailyScheduleView>();
        }
    }

}
