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
    public class JobsDirectiveTime: ISheduleClass
    {
        private readonly IGetWorkerStratagy _getWorkerStratagy;
        private readonly INextJobStratagy _nextJobStratagy;
        private readonly IModifyStratagy _modifyStratagy;

        public JobsDirectiveTime(IGetWorkerStratagy getWorkerStratagy = null, INextJobStratagy nextJobStratagy = null, IModifyStratagy criticalJobStratagy = null)
        {
            _getWorkerStratagy = getWorkerStratagy ?? new GetWorkerStratagy.MinTimeOfWork();
            _nextJobStratagy = nextJobStratagy ?? new NextJobStratagies.MaxMulctStratagy();
            _modifyStratagy = criticalJobStratagy ?? new PenaltyStratagy();
        }

        public long GetPenalty(IProject proj,  Plan plan)
        {
            var penalty = 0L;
            foreach (var job in proj.Jobs)
            {
                penalty += GetPenalty(job, plan);
            }
            return penalty;
        }

        public long GetPenalty(Job job, Plan plan)
        {
            var timeEnd = job.IsCompleted
                ? job.TimeEnd
                : plan.Time;
            var lateness = timeEnd - job.LateTime;
                lateness = lateness > 0 ? lateness : 0;
            var penalty = job.Mulct * lateness;
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

        public void ModifyProject(IProject project, Plan plan)
        {
            _modifyStratagy.ModifyProject(project, plan);
        }
    }
}
