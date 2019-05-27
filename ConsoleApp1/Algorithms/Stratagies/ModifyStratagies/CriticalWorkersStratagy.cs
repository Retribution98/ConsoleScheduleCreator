using ConsoleScheduleCreator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy
{
    public class CriticalWorkersStratagy : IModifyStratagy
    {
        public void ModifyProject(IProject project, Plan plan)
        {
            var maxValue = project.Workers.Max(w => plan.GetTimeInProcess(w));
            project.Workers.ForEach(w => w.Priority = maxValue - plan.GetTimeInProcess(w) + 1);
        }
    }
}
