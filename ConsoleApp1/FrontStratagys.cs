using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public interface IFrontStratagy
    {
        Job GetJob(List<Job> jobs);
    }

    public class GredyStratagy : IFrontStratagy
    {
        public Job GetJob(List<Job> jobs)
        {
            return jobs.First();
        }
    }
}
