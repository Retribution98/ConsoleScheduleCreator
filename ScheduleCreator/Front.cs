using ScheduleApp.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleCreator.Utils;

namespace ScheduleCreator
{
    public class Front
    {
        public List<JobDto> Jobs { get; }

        public Front(List<JobDto> jobs, DateTime time)
        {
            if (jobs != null)
            {
                Jobs = new List<JobDto>();
                foreach (var job in jobs)
                {
                    if (job.IsReady(time))
                    {
                        Jobs.Add(job);
                    }
                }
            }
        }
        public JobDto GetNextJob(IFrontStratagy stratagyNextJob)
        {
            return stratagyNextJob.GetJob(Jobs);
        }
    }
}
