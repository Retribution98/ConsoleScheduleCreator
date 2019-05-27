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
    public interface ICriterion : IGetWorkerStratagy, INextJobStratagy, IModifyStratagy
    {
        long GetCriterion(IProject proj, Plan plan);
    }
}
