using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.NextJobStratagies
{
    public class FirstlyStratagy : INextJobStratagy
    {
        public Job GetJob(List<Job> jobs)
        {
            return jobs != null ? jobs.FirstOrDefault() : null;
        }
    }
}
