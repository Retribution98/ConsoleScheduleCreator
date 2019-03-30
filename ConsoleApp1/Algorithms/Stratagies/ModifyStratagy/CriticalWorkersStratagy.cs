using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy
{
    class CriticalWorkersStratagy : IModifyStratagy
    {
        public void ModifyProject(Project project)
        {
            var maxValue = project.Workers.Max(w => project.Schedule.Planner.GetTimeInProcess(w));
            project.Workers.ForEach(w => w.Priority = maxValue - project.Schedule.Planner.GetTimeInProcess(w) + 1);
        }
    }
}
