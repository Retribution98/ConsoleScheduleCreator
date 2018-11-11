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

        [Fact]
        public void AppointJob_1Job_CompleteTrue()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Job job1 = new Job("Test", 1, 1, 10, 10);
            jobs.Add(job1);
            string[] workersName = new string[1] { "Vasya" };
            int[,] workersTime = new int[1, 1] { { 3 } };
            Project proj = new Project("Test", 0, 100, jobs, workersName, workersTime);
            Job[,] plan = new Job[1, 5];

            Object[] arg = new Object[4];
            arg[0] = proj;
            arg[1] = plan;
            arg[2] = 1;
            arg[3] = job1;

            object penalty = t.InvokeMember("AppointJob",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);

            Assert.True(job1.Completed);
        }

        [Fact]
        public void AppointJob_1Job_Penalty()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Job job1 = new Job("Test", 1, 1, 1, 10);
            jobs.Add(job1);
            string[] workersName = new string[1] { "Vasya" };
            int[,] workersTime = new int[1, 1] { { 3 } };
            Project proj = new Project("Test", 0, 100, jobs, workersName, workersTime);
            Job[,] plan = new Job[1, 5];

            Object[] arg = new Object[4];
            arg[0] = proj;
            arg[1] = plan;
            arg[2] = 1;
            arg[3] = job1;

            object actual = t.InvokeMember("AppointJob",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);

            Int64 expected = 20;

            Assert.Equal(expected, (Int64)actual);
        }

        [Fact]
        public void AppointJob_1Job_AddProcessForWorker()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Job job1 = new Job("Test", 1, 1, 1, 10);
            jobs.Add(job1);
            string[] workersName = new string[3] { "Vasya", "Petya", "Vova" };
            int[,] workersTime = new int[3, 1] { { 5 }, { 3 }, { 3 } };
            Project proj = new Project("Test", 0, 100, jobs, workersName, workersTime);
            Job[,] plan = new Job[3, 5];

            Object[] arg = new Object[4];
            arg[0] = proj;
            arg[1] = plan;
            arg[2] = 1;
            arg[3] = job1;

            object penalty = t.InvokeMember("AppointJob",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);


            int actual = proj.Workers[1].TimeInProcess;
            int expected = 3;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AppointJob_1Job_AddToPlan()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Job job1 = new Job("Test", 1, 1, 1, 10);
            jobs.Add(job1);
            string[] workersName = new string[1] { "Vasya" };
            int[,] workersTime = new int[1, 1] { { 3 } };
            Project proj = new Project("Test", 0, 100, jobs, workersName, workersTime);
            Job[,] plan = new Job[1, 5];

            Object[] arg = new Object[4];
            arg[0] = proj;
            arg[1] = plan;
            arg[2] = 1;
            arg[3] = job1;

            object penalty = t.InvokeMember("AppointJob",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);

            Job[,] actual = plan;
            Job[,] expected = new Job[1,5]{ {null,job1,job1,job1,null } };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HaveNotCompletedJob_1Job_True()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Job job1 = new Job("Test", 1, 1, 10, 10);
            jobs.Add(job1);
            string[] workersName = new string[0]; 
            int[,] workersTime = new int[1, 0];
            Project proj = new Project("Test", 0, 100, jobs, workersName, workersTime);

            Object[] arg = new Object[1];
            arg[0] = proj;

            object actual = t.InvokeMember("HaveNotCompletedJob",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);

            Assert.True((bool)actual);
        }

        [Fact]
        public void HaveNotCompletedJob_1CompletedJob_False()
        {
            FrontAlgorithm obj = new FrontAlgorithm(new FirstlyStratagy());
            Type t = typeof(FrontAlgorithm);

            List<Job> jobs = new List<Job>();
            Job job1 = new Job("Test", 1, 1, 10, 10);
            job1.Complete(1, 1);
            jobs.Add(job1);
            string[] workersName = new string[0];
            int[,] workersTime = new int[1, 0];
            Project proj = new Project("Test", 0, 100, jobs, workersName, workersTime);

            Object[] arg = new Object[1];
            arg[0] = proj;

            object actual = t.InvokeMember("HaveNotCompletedJob",
                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                    BindingFlags.Public | BindingFlags.Instance,
                                    null, obj, arg);

            Assert.False((bool)actual);
        }


    }
}
