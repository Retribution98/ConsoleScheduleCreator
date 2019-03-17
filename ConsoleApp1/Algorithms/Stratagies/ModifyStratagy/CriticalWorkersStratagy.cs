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
            var criticalWorkers = GetCriticalWorkers(project);
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Critical workers:" + String.Join(", ", criticalWorkers.Select(w => w.Name)));
            Console.ResetColor();
            foreach (var worker in criticalWorkers)
            {
                worker.Priority++;
            }
        }

        private IEnumerable<Worker> GetCriticalWorkers(Project project)
        {
            var minTimeInProcess = project.Workers.Min(w => w.TimeInProcess);
            return project.Workers.Where(w => w.TimeInProcess == minTimeInProcess);
        }
    }
}
