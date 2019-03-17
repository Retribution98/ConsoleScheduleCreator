using ConsoleScheduleCreator.NextJobStratagies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms
{
    public class Front
    {
        public List<Job> Jobs { get; }

        public Front(List<Job> jobs, int time)
        {
            if (jobs != null)
            {
                Jobs = new List<Job>();
                foreach (Job job in jobs)
                {
                    if (job.Ready(time))
                    {
                        Jobs.Add(job);
                    }
                }
            }
        }
        public Job GetNextJob(INextJobStratagy stratagyNextJob)
        {
            return stratagyNextJob.GetJob(Jobs);
        }
    }
}
