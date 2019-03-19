using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleScheduleCreator.Entities;

namespace ConsoleScheduleCreator.Algorithms.Stratagies.GetWorkerStratagy
{
    public class FirstlyStratagy: IGetWorkerStratagy
    {
        public Worker GetWorker(Job job, Plan plan, int time)
        {
            return plan != null && plan.Workers != null 
                ? plan.Workers
                    .Where(w => plan[w, time] == null)
                    .FirstOrDefault() 
                : null;
        }
    }
}
