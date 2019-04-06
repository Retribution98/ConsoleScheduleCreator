using ConsoleScheduleCreator.Entities;
using System.Collections.Generic;

namespace ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy
{
    public interface IModifyStratagy
    {
        void ModifyProject(IProject project);
    }
}