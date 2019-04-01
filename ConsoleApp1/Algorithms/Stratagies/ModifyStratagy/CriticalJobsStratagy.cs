using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy
{
    class PenaltyStratagy: IModifyStratagy
    {
        private IEnumerable<Job> GetCriticalJobs(Project proj)
        {
            var lastJob = proj.Jobs.First();
            foreach (var job in proj.Jobs)
            {
                if (job.TimeEnd > lastJob.TimeEnd && job.TimeStart != job.EarlyTime)
                    lastJob = job;
            }
            var result = GetCriticalJobs(lastJob);
            return result;
        }

        private IEnumerable<Job> GetCriticalJobs(Job job)
        {
            var criticalPrev = job.Previos.Where(j => j.TimeEnd == job.TimeStart - 1 && j.TimeStart != j.EarlyTime).ToList();
            if (criticalPrev.Any())
            {
                var result = new List<Job> { job };
                criticalPrev.ForEach(j => result.AddRange(GetCriticalJobs(j)));
                return result;
            }
            else
            {
                return new List<Job> { job };
            }
        }

        public void ModifyProject(Project project)
        {
            var criticalJobs = GetCriticalJobs(project);
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Critical jobs:" + String.Join(", ", criticalJobs.Select(j => j.Id)));
            Console.ResetColor();
            foreach (var job in criticalJobs)
            {
                job.Priority++;
            }
        }
    }
}
