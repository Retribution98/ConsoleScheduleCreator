using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.CriticalJobStratagy
{
    class GreedyStatagy: ICriticalJobStratagy
    {
        public IEnumerable<Job> GetCriticalJobs(Project proj)
        {
            var lastJob = proj.Jobs.First();
            foreach (var job in proj.Jobs)
            {
                if (job.FinalPenalty > lastJob.FinalPenalty && job.TimeStart != job.EarlyTime)
                    lastJob = job;
            }
            var result = GetCriticalJobs(lastJob);
            return result;
        }

        public IEnumerable<Job> GetCriticalJobs(Job job)
        {
            var criticalPrev = job.Previos.Where(j => j.TimeEnd == job.TimeStart - 1 && j.TimeStart != j.EarlyTime).ToList();
            if (criticalPrev.Any())
            {
                var result = new List<Job>();
                criticalPrev.ForEach(j => result.AddRange(GetCriticalJobs(j)));
                return result;
            }
            else
            {
                return new List<Job> { job };
            }
        }

        public void ModifyJobs(IEnumerable<Job> jobs)
        {
            foreach (var job in jobs)
            {
                job.Mulct = job.Mulct * 2;
            }

        }
    }
}
