using ScheduleApp.DataAccess.DTO;
using System;

namespace ScheduleCreator
{
    public interface IAlgorithm
    {
        Schedule CreateShedule(ProjectDto proj, DateTime timeStart, DateTime timeEnd, PeriodUnit periodUnit);
    }
}
