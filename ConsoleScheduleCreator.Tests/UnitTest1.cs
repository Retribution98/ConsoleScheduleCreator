using System;
using System.Collections.Generic;
using Xunit;
using ConsoleScheduleCreator;

namespace ConsoleScheduleCreator.Tests
{
    public class ProjectTests
    {
        [Fact]
        public void ReadyJobsTests_Null_EmptyList()
        {
            Project proj = new Project("Test", 0, 0, 0, 0, null, null);

            List<Job> actual = proj.ReadyJobs(10);

            Assert.Empty(actual);
        }

        [Fact]
        public void ReadyJobsTests_1Job_1Job()
        {
            Project proj = new Project("Test", 0, 100, 1, 0, null, null);
            proj.AddJob("TestJob", 0, 100, 100, 0, null);
            Assert.Equal(new Job("TestJob", 1, 0, 100, 100).ToString(), proj.ReadyJobs(10)[0].ToString());

            List<Job> actual = proj.ReadyJobs(10);

            Assert.Single(actual);
        }

        [Fact]
        public void ReadyJobsTests_negativeTime_ArgumentException()
        {
            Project proj = new Project("Test", 0, 0, 0, 0, null, null);
            Action testCode = () => proj.ReadyJobs(-1);
            Assert.Throws<ArgumentException>(testCode);
        }
    }
}
