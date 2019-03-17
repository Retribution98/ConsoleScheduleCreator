using ConsoleScheduleCreator.Algorithms.Stratagies.GetWorkerStratagy;
using ConsoleScheduleCreator.Entities;
using ConsoleScheduleCreator.NextJobStratagies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms.SheduleClasses
{
    class EqualWorker: ISheduleClass
    {
        private readonly IGetWorkerStratagy _getWorkerStratagy;
        private readonly INextJobStratagy _nextJobStratagy;
        private readonly ICriticalJobStratagy _criticalJobStratagy;

        public EqualWorker(IGetWorkerStratagy getWorkerStratagy = null, INextJobStratagy nextJobStratagy = null, ICriticalJobStratagy criticalJobStratagy = null)
        {
            _getWorkerStratagy = getWorkerStratagy ?? new EqualTimeInProces();
            _nextJobStratagy = nextJobStratagy ?? new FirstlyStratagy();
            _criticalJobStratagy = criticalJobStratagy;
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

        public long GetPenalty(Project proj, Plan plan)
        {
            throw new NotImplementedException();
        }
    }
}
