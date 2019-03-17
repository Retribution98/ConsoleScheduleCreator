using ConsoleScheduleCreator.NextJobStratagies;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleScheduleCreator.Tests.NextJobStratagyTests
{
    public class GreedyStratagyTests
    {
        [Fact]
        void GetJobTest_Null_Null()
        {
            var stratagy = new MaxMulctStratagy();

            var actual = stratagy.GetJob(null);

            Assert.Null(actual);
        }

        [Fact]
        void GetJobTest_1Job_Job()
        {
            var stratagy = new MaxMulctStratagy();
            var jobs = new List<Job>();
            var test = new Job("Test", 1, 1, 1, 1);
            jobs.Add(test);

            var actual = stratagy.GetJob(jobs);
            var expected = test;

            Assert.Equal(expected, actual);
        }

        [Fact]
        void GetJobTest_2Job_Job2()
        {
            var stratagy = new MaxMulctStratagy();
            var jobs = new List<Job>();
            var test1 = new Job("Test", 1, 1, 1, 1);
            var test2 = new Job("Test", 2, 2, 2, 2);

            jobs.Add(test1);
            jobs.Add(test2);

            var actual = stratagy.GetJob(jobs);
            var expected = test2;

            Assert.Equal(expected, actual);
        }

        [Fact]
        void GetJobTest_3Job_Job2()
        {
            var stratagy = new MaxMulctStratagy();
            var jobs = new List<Job>();
            var test1 = new Job("Test", 1, 1, 1, 1);
            var test2 = new Job("Test", 2, 2, 2, 2);
            var test3 = new Job("Test", 3, 2, 2, 2);

            jobs.Add(test1);
            jobs.Add(test2);
            jobs.Add(test3);

            var actual = stratagy.GetJob(jobs);
            var expected = test2;

            Assert.Equal(expected, actual);
        }
    }
}
