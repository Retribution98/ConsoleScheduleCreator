using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.NextJobStratagies
{
    public class FirstlyStratagy : INextJobStratagy
    {
        public Job GetJob(IEnumerable<Job> jobs)
        {
            return jobs?.FirstOrDefault();
        }
    }
}
