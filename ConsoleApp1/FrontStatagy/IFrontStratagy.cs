using ScheduleApp.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleCreator
{
    public interface IFrontStratagy
    {
        JobDto GetJob(List<JobDto> jobs);
    }
}
