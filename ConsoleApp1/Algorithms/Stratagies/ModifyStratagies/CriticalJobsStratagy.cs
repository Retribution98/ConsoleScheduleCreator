using ConsoleScheduleCreator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy
{
    public class PenaltyStratagy: IModifyStratagy
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
            var result = new List<Job>();
            AddCriticalJobs(lastJob, result);
            return result;
        }

        private void AddCriticalJobs(Job job, List<Job> criticalJobs)
        {
            criticalJobs.Add(job);
            var criticalPrev = job.Previos.Where(j => _jobsTimeEnd[j].end == _jobsTimeEnd[job].start - 1 && _jobsTimeEnd[j].start != j.EarlyTime).ToList();
            if (criticalPrev.Any())
            {
                criticalPrev.ForEach(j =>
                {
                    if (!criticalJobs.Contains(j))
                    {
                        AddCriticalJobs(j, criticalJobs);
                    }
                });
            }
        }

        public void ModifyProject(IProject project, Plan plan)
        {
            var print = new PrinterToDebug();
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
            print.PrintLn("Critical jobs:" + String.Join(", ", criticalJobs.Select(j => j.Name)));
            Console.ResetColor();
            foreach (var job in criticalJobs)
            {
                job.Priority++;
            }
        }
    }
}
