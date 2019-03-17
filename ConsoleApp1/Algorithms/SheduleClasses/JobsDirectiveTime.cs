using ConsoleScheduleCreator.Algorithms.SheduleClasses;
using ConsoleScheduleCreator.Entities;
using ConsoleScheduleCreator.NextJobStratagies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    class JobsDirectiveTime: ISheduleClass
    {
        private readonly IGetWorkerStratagy _getWorkerStratagy;
        private readonly INextJobStratagy _nextJobStratagy;
        private readonly ICriticalJobStratagy _criticalJobStratagy;

        public JobsDirectiveTime()
        {
            _getWorkerStratagy = new GetWorkerStratagy.GreedyStatagy();
            _nextJobStratagy = new NextJobStratagies.GreedyStratagy();
            _criticalJobStratagy = new CriticalJobStratagy.GreedyStatagy();
        }

        public long GetPenalty(Project proj,  Plan plan)
        {
            var penalty = 0L;
            foreach (var job in proj.Jobs)
            {
                if (job.Completed == true)
                {
                    penalty += job.FinalPenalty;
                }
                else penalty += job.Mulct * plan.Time;
            }
            return penalty;
        }

        public Job GetJob(List<Job> jobs)
        {
            return _nextJobStratagy.GetJob(jobs);
        }

        public Worker GetWorker(Job job, Plan plan, int time)
        {
            return _getWorkerStratagy.GetWorker(job, plan, time);
        }

        public IEnumerable<Job> GetCriticalJobs(Project proj)
        {
            return _criticalJobStratagy.GetCriticalJobs(proj);
        }

        public IEnumerable<Job> GetCriticalJobs(Job proj)
        {
            return _criticalJobStratagy.GetCriticalJobs(proj);
        }

        public void ModifyJobs(IEnumerable<Job> jobs)
        {
            _criticalJobStratagy.ModifyJobs(jobs);
        }
    }
}
