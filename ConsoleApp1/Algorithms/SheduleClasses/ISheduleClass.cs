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
    public interface ISheduleClass : IGetWorkerStratagy, INextJobStratagy, IModifyStratagy
    {
        long GetPenalty(Project proj, Plan plan);
    }
}
