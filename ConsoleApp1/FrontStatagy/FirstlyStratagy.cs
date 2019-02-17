using ScheduleApp.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCreator
{
    public class FirstlyStratagy : IFrontStratagy
    {
        public JobDto GetJob(List<JobDto> jobs)
        {
            if ((jobs == null)||(!jobs.Any()))
                return null;
            return jobs.First();
        }
    }
}
