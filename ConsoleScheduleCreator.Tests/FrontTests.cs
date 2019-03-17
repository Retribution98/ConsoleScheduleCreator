using ConsoleScheduleCreator.Algorithms;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleScheduleCreator.Tests
{
    public class FrontTests
    {
        [Fact]
        public void GetReadyJobs_1Job_Job()
        {
            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 1, 1);
            jobs.Add(job1);

            var actual = new Front(jobs, 2).Jobs;
            var expected = new List<Job>
            {
                job1
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetReadyJobs_Empty_EmptyList()
        {
            var jobs = new List<Job>();

            var actual = new Front(jobs, 2).Jobs;

            Assert.Empty(actual);
        }

        [Fact]
        public void GetReadyJobs_CompletedJob_Empty()
        {
            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 1, 1);
            job1.Complete(1, 1);
            jobs.Add(job1);

            var actual = new Front(jobs, 2).Jobs;

            Assert.Empty(actual);
        }

    }
}
