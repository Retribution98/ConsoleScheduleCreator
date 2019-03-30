using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ConsoleScheduleCreator.Entities;
using System.Reflection;

namespace ConsoleScheduleCreator.Tests.AppointJobStratagyTests
{
    public class GreedyStratagyTest
    {
        [Fact]
        public void AppointJob_1Job_CompleteTrue()
        {
            var stratagy = new GetWorkerStratagy.MinTimeOfWork();

            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 1, 10);
            jobs.Add(job1);

            var workersName = new string[3] { "Vasya", "Petya", "Vova" };
            var workersTime = new int[3, 1] { { 5 }, { 3 }, { 3 } };
            var proj = new Project("Test", 0, 100, jobs, workersName, workersTime);
            var plan = new Plan(proj.Workers, proj.Late);
            plan.AppointJob(job1, proj.Workers.Where(x => x.Name == "Petya").FirstOrDefault(), 1);

            var expectedPenalty = 20L;

            Assert.True(job1.Completed);
            Assert.Equal(expectedPenalty, job1.FinalPenalty);
            //Assert.Equal(new Job[1, 5] {{ job1, job1, job1, null, null }}, plan);
        }

        [Fact]
        public void AppointJob_1Job_AddProcessForWorker()
        {
            var stratagy = new GetWorkerStratagy.MinTimeOfWork();

            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 1, 10);
            jobs.Add(job1);
            var workersName = new string[3] { "Vasya", "Petya", "Vova" };
            var workersTime = new int[3, 1] { { 5 }, { 3 }, { 3 } };
            var proj = new Project("Test", 0, 5, jobs, workersName, workersTime);
            var plan = new Plan(proj.Workers, proj.Late);

            plan.AppointJob(job1, proj.Workers.Where(x => x.Name == "Petya").FirstOrDefault(), 1);

            int actual = plan.GetTimeInProcess(proj.Workers[1]);
            int expected = 3;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AppointJob_1Job_AddToPlan()
        {
            var stratagy = new GetWorkerStratagy.MinTimeOfWork();

            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 1, 10);
            jobs.Add(job1);
            var workersName = new string[1] { "Vasya" };
            var workersTime = new int[1, 1] { { 3 } };
            var proj = new Project("Test", 0, 5, jobs, workersName, workersTime);
            var plan = new Plan(proj.Workers, proj.Late);

            plan.AppointJob(job1, proj.Workers.FirstOrDefault(), 1);

            var actual = (Job[,])typeof(Plan).GetField("_jobs", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(plan);
            var expected = new Job[1, 5] { { null, job1, job1, job1, null } };

            Assert.Equal(expected, actual);
        }
    }
}
