using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace ConsoleScheduleCreator.Tests
{
    public class AlgorithmsTests
    {
        [Fact]
        public void GetReadyJobs_1Job_Job()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Job job1 = new Job("Test", 1, 1, 1, 1);
            jobs.Add(job1);
            Object[] arg = new Object[2];
            arg[0] = jobs;
            arg[1] = 2;

            object actual = t.InvokeMember("GetReadyJobs",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);
            List<Job> expected = new List<Job>
            {
                job1
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetReadyJobs_Empty_EmptyList()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Object[] arg = new Object[2];
            arg[0] = jobs;
            arg[1] = 2;

            object actual = t.InvokeMember("GetReadyJobs",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);

            Assert.Empty((List<Job>)actual);
        }

        [Fact]
        public void GetReadyJobs_CompletedJob_Empty()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Job job1 = new Job("Test", 1, 1, 1, 1);
            job1.Complete(1, 1);
            jobs.Add(job1);
            Object[] arg = new Object[2];
            arg[0] = jobs;
            arg[1] = 2;

            object actual = t.InvokeMember("GetReadyJobs",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);
            
            Assert.Empty((List<Job>)actual);
        }
    }
}
