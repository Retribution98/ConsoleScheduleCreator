using ConsoleScheduleCreator.Algorithms.SheduleClasses;
using ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy;
using ConsoleScheduleCreator.Entities;
using ConsoleScheduleCreator.NextJobStratagies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public abstract class CriterionAbstract : ICriterion
    {
        private readonly IGetWorkerStratagy _getWorkerStratagy;
        private readonly INextJobStratagy _nextJobStratagy;
        private readonly IModifyStratagy _modifyStratagy;

        public CriterionAbstract(IGetWorkerStratagy getWorkerStratagy, INextJobStratagy nextJobStratagy, IModifyStratagy criticalJobStratagy)
        {
            _getWorkerStratagy = getWorkerStratagy ?? throw new ArgumentNullException("getWorkerStratagy");
            _nextJobStratagy = nextJobStratagy ?? throw new ArgumentNullException("nextJobStratagy");
            _modifyStratagy = criticalJobStratagy ?? throw new ArgumentNullException("criticalJobStratagy");
        }

        public abstract long GetCriterion(IProject proj, Plan plan);

        public Job GetJob(IEnumerable<Job> jobs)
        {
            return _nextJobStratagy.GetJob(jobs);
        }

        public Worker GetWorker(Job job, Plan plan, int time)
        {
            return _getWorkerStratagy.GetWorker(job, plan, time);
        }

        public void ModifyProject(IProject project, Plan plan)
        {
            _modifyStratagy.ModifyProject(project, plan);
        }
    }
}
