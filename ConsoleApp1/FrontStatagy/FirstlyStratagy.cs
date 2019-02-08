using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public class FirstlyStratagy : IFrontStratagy
    {
        public Job GetJob(List<Job> jobs)
        {
            if (jobs == null) return null;
            return jobs.First();
        }
    }
}
