using System.Collections.Generic;

namespace ConsoleScheduleCreator
{
    public interface ICriticalJobStratagy
    {
        IEnumerable<Job> GetCriticalJobs(Project proj);
        IEnumerable<Job> GetCriticalJobs(Job proj);
        void ModifyJobs(IEnumerable<Job> jobs);
    }
}