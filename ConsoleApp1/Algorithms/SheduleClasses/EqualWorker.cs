using ConsoleScheduleCreator.Algorithms.Stratagies.GetWorkerStratagy;
using ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy;
using ConsoleScheduleCreator.Entities;
using ConsoleScheduleCreator.NextJobStratagies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms.SheduleClasses
{
    public class EqualWorker: ISheduleClass
    {
        private readonly IGetWorkerStratagy _getWorkerStratagy;
        private readonly INextJobStratagy _nextJobStratagy;
        private readonly IModifyStratagy _modifyStratagy;

        public EqualWorker(IGetWorkerStratagy getWorkerStratagy = null, INextJobStratagy nextJobStratagy = null, IModifyStratagy criticalJobStratagy = null)
        {
            _getWorkerStratagy = getWorkerStratagy ?? new EqualTimeInProces();
            _nextJobStratagy = nextJobStratagy ?? new FirstlyStratagy();
            _modifyStratagy = criticalJobStratagy ?? new CriticalWorkersStratagy();
        }

        public Job GetJob(List<Job> jobs)
        {
            return _nextJobStratagy.GetJob(jobs);
        }

        public Worker GetWorker(Job job, Plan plan, int time)
        {
            return _getWorkerStratagy.GetWorker(job, plan, time);
        }

        public long GetPenalty(Project proj, Plan plan)
        {
            throw new NotImplementedException();
        }

        public void ModifyProject(Project project)
        {
            _modifyStratagy.ModifyProject(project);
        }
    }
}
