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
            var maxValue = project.Workers.Max(w => w.TimeInProcess);
            project.Workers.ForEach(w => w.Priority = maxValue - w.TimeInProcess + 1);
        }
    }
}
