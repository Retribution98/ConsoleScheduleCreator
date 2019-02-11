using ScheduleApp.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleCreator
{
    public class GreedyStratagy : IFrontStratagy
    {
        public JobDto GetJob(List<JobDto> jobs)
        {
            if ((jobs == null) || (!jobs.Any()))
                return null;
            jobs.Sort(
                (x, y) =>
                {
                    if (x.Mulct > y.Mulct)
                        return -1;
                    else if (x.Mulct < y.Mulct)
                        return 1;
                    else return 0;
                });
            return jobs.First();
        }
    }
}
