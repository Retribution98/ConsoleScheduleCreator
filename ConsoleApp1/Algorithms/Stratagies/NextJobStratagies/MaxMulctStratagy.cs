using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.NextJobStratagies
{
    public class MaxMulctStratagy : INextJobStratagy
    {
        public Job GetJob(IEnumerable<Job> jobs)
        {
            if (jobs == null) return null;
            return jobs
                .OrderByDescending(j => j.Priority)
                .ThenByDescending(j => j.Mulct)
                .FirstOrDefault();
        }
    }
}
