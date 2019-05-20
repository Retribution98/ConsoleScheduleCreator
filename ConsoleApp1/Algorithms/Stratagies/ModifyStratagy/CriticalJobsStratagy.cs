using ConsoleScheduleCreator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy
{
    class PenaltyStratagy: IModifyStratagy
    {
        private IDictionary<Job, (int? start, int? end)> _jobsTimeEnd;

        private IEnumerable<Job> GetCriticalJobs(IProject proj)
        {
            var lastJob = proj.Jobs.First();
            foreach (var job in proj.Jobs)
            {
                if (_jobsTimeEnd[job].end > _jobsTimeEnd[lastJob].end && _jobsTimeEnd[job].start != job.EarlyTime)
                    lastJob = job;
            }
            var result = GetCriticalJobs(lastJob);
            return result;
        }

        private IEnumerable<Job> GetCriticalJobs(Job job)
        {
            var criticalPrev = job.Previos.Where(j => _jobsTimeEnd[j].end == _jobsTimeEnd[job].start - 1 && _jobsTimeEnd[j].start != j.EarlyTime).ToList();
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

        public void ModifyProject(IProject project, Plan plan)
        {
            _jobsTimeEnd = new Dictionary<Job, (int?, int?)>();
            foreach(var job in project.Jobs)
            {
                var times = plan.GetTimesJobExecute(job);
                if (times.Any())
                {
                    _jobsTimeEnd.Add(job, (times.Min(), times.Max()));
                }
                else
                {
                    _jobsTimeEnd.Add(job, (null, null));
                }
            }
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
