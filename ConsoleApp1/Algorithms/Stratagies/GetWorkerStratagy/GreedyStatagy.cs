using ConsoleScheduleCreator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.GetWorkerStratagy
{
    public class GreedyStatagy :IGetWorkerStratagy
    {
        public Worker GetWorker(Job job, Plan plan, int time)
        {
            var selectWorker = plan.Workers
                .Where(w => plan[w, time] == null)
                .OrderBy(w => w.TimeOfWork[job.Id])
                .ThenBy(w => w.TimeInProcess)
                .FirstOrDefault();

            return selectWorker;
        }
    }
}
