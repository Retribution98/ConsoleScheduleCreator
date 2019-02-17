using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleScheduleCreator.Tests
{
    public class FirstlyStratagyTests
    {
        [Fact]
        void GetJobTest_Null_Null()
        {
            IFrontStratagy stratagy = new FirstlyStratagy();

            Job actual = stratagy.GetJob(null);

            Assert.Null(actual);
        }

        [Fact]
        void GetJobTest_1Job_Job()
        {
            IFrontStratagy stratagy = new FirstlyStratagy();
            List<Job> jobs = new List<Job>();
            Job test = new Job("Test", 1, 1, 1, 1);
            jobs.Add(test);

            Job actual = stratagy.GetJob(jobs);
            Job expected = test;

            Assert.Equal(expected, actual);
        }

        [Fact]
        void GetJobTest_2Job_Job()
        {
            IFrontStratagy stratagy = new FirstlyStratagy();
            List<Job> jobs = new List<Job>();
            Job test1 = new Job("Test", 1, 1, 1, 1);
            Job test2 = new Job("Test", 2, 2, 2, 2);

            jobs.Add(test1);
            jobs.Add(test2);

            Job actual = stratagy.GetJob(jobs);
            Job expected = test1;

            Assert.Equal(expected, actual);
        }
    }

    public class GreedyStratagyTests
    {
        [Fact]
        void GetJobTest_Null_Null()
        {
            IFrontStratagy stratagy = new GreedyStratagy();

            Job actual = stratagy.GetJob(null);

            Assert.Null(actual);
        }

        [Fact]
        void GetJobTest_1Job_Job()
        {
            IFrontStratagy stratagy = new GreedyStratagy();
            List<Job> jobs = new List<Job>();
            Job test = new Job("Test", 1, 1, 1, 1);
            jobs.Add(test);

            Job actual = stratagy.GetJob(jobs);
            Job expected = test;

            Assert.Equal(expected, actual);
        }

        [Fact]
        void GetJobTest_2Job_Job2()
        {
            IFrontStratagy stratagy = new GreedyStratagy();
            List<Job> jobs = new List<Job>();
            Job test1 = new Job("Test", 1, 1, 1, 1);
            Job test2 = new Job("Test", 2, 2, 2, 2);

            jobs.Add(test1);
            jobs.Add(test2);

            Job actual = stratagy.GetJob(jobs);
            Job expected = test2;

            Assert.Equal(expected, actual);
        }

        [Fact]
        void GetJobTest_3Job_Job2()
        {
            IFrontStratagy stratagy = new GreedyStratagy();
            List<Job> jobs = new List<Job>();
            Job test1 = new Job("Test", 1, 1, 1, 1);
            Job test2 = new Job("Test", 2, 2, 2, 2);
            Job test3 = new Job("Test", 3, 2, 2, 2);

            jobs.Add(test1);
            jobs.Add(test2);
            jobs.Add(test3);

            Job actual = stratagy.GetJob(jobs);
            Job expected = test2;

            Assert.Equal(expected, actual);
        }
    }
}
