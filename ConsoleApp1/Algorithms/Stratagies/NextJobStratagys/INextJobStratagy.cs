using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.NextJobStratagies
{
    public interface INextJobStratagy
    {
        Job GetJob(List<Job> jobs);
    }
}
