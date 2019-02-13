using ScheduleApp.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleCreator.Utils
{
    static internal class ScheduleUtils
    {
        public  static bool IsReady(this JobDto job, DateTime time)
        {
            if (job.TimeEnd.HasValue || job.TimeEnd.HasValue || job.EarlyTime > time)
            {
                return false;
            }
            foreach (var parent in job.Parents)
            {
                if (!parent.TimeEnd.HasValue || parent.TimeEnd > time)
                {
                    return false;
                }
            }
            return true;
        }
    }

}
