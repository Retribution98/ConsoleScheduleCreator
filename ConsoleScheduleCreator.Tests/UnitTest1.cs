using System;
using System.Collections.Generic;
using Xunit;

namespace ConsoleScheduleCreator.Tests
{
    public class ProjectTests
    {
        [Fact]
        public void ReadyJobsTests_Null_EmptyList()
        {
            Project proj = new Project("Test", 0, 0, null, null, null);

            List<Job> actual = proj.ReadyJobs(10);

            Assert.Empty(actual);
        }

        [Fact]
        public void ReadyJobsTests_1Job_1Job()
        {
            List<Job> jobs = new List<Job>();
            jobs.Add(new Job("TestJob", 1, 0, 100, 10));
            Project proj = new Project("Test", 0, 100, jobs, null, null);
            
            Assert.Equal(new Job("TestJob", 1, 0, 100, 100).ToString(), proj.ReadyJobs(10)[0].ToString());

            List<Job> actual = proj.ReadyJobs(10);

            Assert.Single(actual);
        }

    }
}
