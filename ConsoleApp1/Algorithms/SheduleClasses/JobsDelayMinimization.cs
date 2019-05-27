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
    public class JobsDelayMinimization: CriterionAbstract
    {
        public JobsDelayMinimization()
            : base(new GetWorkerStratagy.MinTimeOfWork(), 
                  new NextJobStratagies.MaxMulctStratagy(), 
                  new PenaltyStratagy())
        {
        }

        public JobsDelayMinimization(IGetWorkerStratagy getWorkerStratagy, 
            INextJobStratagy nextJobStratagy, 
            IModifyStratagy criticalJobStratagy)
            : base(getWorkerStratagy, nextJobStratagy, criticalJobStratagy)
        {
        }

        public override long GetCriterion(IProject proj,  Plan plan)
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
    }
}
