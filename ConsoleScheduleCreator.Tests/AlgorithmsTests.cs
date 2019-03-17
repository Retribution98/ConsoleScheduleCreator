using ConsoleScheduleCreator.Algorithms;
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
        public void HaveNotCompletedJob_1Job_True()
        {
            var obj = new FrontAlgorithm(null, null);
            var t = typeof(FrontAlgorithm);

            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 10, 10);
            jobs.Add(job1);
            var workersName = new string[0];
            var workersTime = new int[1, 0];
            var proj = new Project("Test", 0, 100, jobs, workersName, workersTime);

            var arg = new Object[1];
            arg[0] = proj;

            var actual = t.InvokeMember("HaveDidntCompleteJob",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);

            Assert.True((bool)actual);
        }

        [Fact]
        public void HaveNotCompletedJob_1CompletedJob_False()
        {
            var obj = new FrontAlgorithm(null, null);
            var t = typeof(FrontAlgorithm);

            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 10, 10);
            job1.Complete(1, 1);
            jobs.Add(job1);
            var workersName = new string[0];
            var workersTime = new int[1, 0];
            var proj = new Project("Test", 0, 100, jobs, workersName, workersTime);

            var arg = new Object[1];
            arg[0] = proj;

            var actual = t.InvokeMember("HaveDidntCompleteJob",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);

            Assert.False((bool)actual);
        }
    }
}
