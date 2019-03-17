using ConsoleScheduleCreator.NextJobStratagies;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleScheduleCreator.Tests.NextJobStratagyTests
{
    public class FirstlyStratagyTests
    {
        [Fact]
        void GetJobTest_Null_Null()
        {
            var stratagy = new FirstlyStratagy();

            var actual = stratagy.GetJob(null);

            Assert.Null(actual);
        }

        [Fact]
        void GetJobTest_1Job_Job()
        {
            var stratagy = new FirstlyStratagy();
            var jobs = new List<Job>();
            var test = new Job("Test", 1, 1, 1, 1);
            jobs.Add(test);

            var actual = stratagy.GetJob(jobs);
            var expected = test;

            Assert.Equal(expected, actual);
        }

        [Fact]
        void GetJobTest_2Job_Job()
        {
            var stratagy = new FirstlyStratagy();
            var jobs = new List<Job>();
            var test1 = new Job("Test", 1, 1, 1, 1);
            var test2 = new Job("Test", 2, 2, 2, 2);

            jobs.Add(test1);
            jobs.Add(test2);

            var actual = stratagy.GetJob(jobs);
            var expected = test1;

            Assert.Equal(expected, actual);
        }
    }
}
