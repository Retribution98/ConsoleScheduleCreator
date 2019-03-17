using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.NextJobStratagies
{
    public class GreedyStratagy : INextJobStratagy
    {
        public Job GetJob(List<Job> jobs)
        {
            if (jobs == null) return null;
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
