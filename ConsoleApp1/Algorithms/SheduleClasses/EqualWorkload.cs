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
    public class EqualWorkload: CriterionAbstract
    {
        public EqualWorkload()
            : base(new EqualTimeInProces(),
                  new NextJobStratagies.FirstlyStratagy(),
                  new CriticalWorkersStratagy())
        {
        }

        public EqualWorkload(IGetWorkerStratagy getWorkerStratagy,
            INextJobStratagy nextJobStratagy,
            IModifyStratagy criticalJobStratagy)
            : base(getWorkerStratagy, nextJobStratagy, criticalJobStratagy)
        {
        }

        public override long GetCriterion(IProject proj, Plan plan)
        {
            var maxTimeInProcess = plan.Workers.Max(w => plan.GetTimeInProcess(w));
            var minTimeInProcess = plan.Workers.Min(w => plan.GetTimeInProcess(w));
            return (long)maxTimeInProcess - minTimeInProcess;
        }
    }
}
